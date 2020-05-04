using BusinessLogic.Exceptions;
using BusinessLogic.Services.Abstractions;
using DataAccess.Repositories.Abstractions;
using DomainModel.ForecastingTasks;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Implementations
{
    public class ForecastingTasksService : IForecastingTasksService
    {
        private readonly IForecastingTasksRepository _forecastingTasksRepository;

        public ForecastingTasksService(IForecastingTasksRepository forecastingTasksRepository)
        {
            _forecastingTasksRepository = forecastingTasksRepository;
        }

        public Task<List<ShortForecastingTaskInfo>> GetAllForecastingTaskEntities()
        {
            return _forecastingTasksRepository.GetAllForecastingTaskEntities();
        }

        public async Task CreateForecastingTaskEntity(string entityName, string description, List<ForecastingTaskFieldDeclaration> declaration)
        {
            entityName = entityName?.Trim();
            description = description?.Trim();
            await ValidateCreateForecastingTaskEntity(entityName, declaration);

            int i = 0;
            foreach (var declarationItem in declaration)
            {
                declarationItem.Id = i++;
                declarationItem.Name = declarationItem.Name?.Trim();
                declarationItem.Description = declarationItem.Description?.Trim();
            }
            await _forecastingTasksRepository.CreateForecastingTaskEntity(entityName, description, declaration);
        }

        public async Task EditForecastingTaskEntity(string oldTaskName, string newTaskName, string newTaskDescription)
        {
            oldTaskName = oldTaskName?.Trim();
            newTaskName = newTaskName?.Trim();
            newTaskDescription = newTaskDescription?.Trim();

            if (!await DoesForecastingTaskEntityExist(oldTaskName))
                throw new DomainErrorException($"Forecasting task with name {oldTaskName} doesn't exist!");

            if (oldTaskName != newTaskName && await DoesForecastingTaskEntityExist(newTaskName))
                throw new DomainErrorException($"Forecasting task with name {newTaskName} already exist!");

            await _forecastingTasksRepository.EditForecastingTaskEntity(oldTaskName, newTaskName, newTaskDescription);

            if (oldTaskName != newTaskName)
                RemoveMLModelFiles(oldTaskName);
        }

        public async Task DeleteForecastingTaskEntity(string entityName)
        {
            entityName = entityName?.Trim();
            if (!await DoesForecastingTaskEntityExist(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            await _forecastingTasksRepository.DeleteForecastingTaskEntity(entityName);

            RemoveMLModelFiles(entityName);
        } 

        public async Task AddForecastingTaskRecord(string entityName, List<ForecastingTaskFieldValue> fields)
        {
            entityName = entityName?.Trim();
            if (!await DoesForecastingTaskEntityExist(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            var taskEntityDeclaration = await _forecastingTasksRepository.GetForecastingTaskFieldsDeclaration(entityName);
            if (taskEntityDeclaration.Count != fields.Select(x => x.FieldId).Distinct().Count())
                throw new DomainErrorException($"Forecasting task with name {entityName} and the request have a different count of fields!");

            for (int i = 0; i < fields.Count; i++)
            {
                if (!taskEntityDeclaration.Any(x => x.Id == fields[i].FieldId))
                    throw new DomainErrorException($"Column {fields[i].FieldId} doesn't exist in forecasting task with name {entityName}!");

                fields[i].Value = fields[i].Value?.Trim();
                var fieldDeclaration = taskEntityDeclaration.Single(x => x.Id == fields[i].FieldId);
                if (fieldDeclaration.Type != FieldType.InformationField && !float.TryParse(fields[i].Value, out _))
                    throw new DomainErrorException($"Field {fieldDeclaration.Name} must to be filled with a number! But was filled with value: {fields[i].Value}");
            }

            await _forecastingTasksRepository.AddForecastingTaskRecord(entityName, fields);
        }

        public async Task DeleteForecastingTaskRecordById(string entityName, string recordId)
        {
            entityName = entityName?.Trim();
            if (!await DoesForecastingTaskEntityExist(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            await _forecastingTasksRepository.DeleteForecastingTaskRecordById(entityName, recordId);
        }

        public async Task<List<ForecastingTaskFieldDeclaration>> GetForecastingTaskEntityDeclaration(string entityName)
        {
            entityName = entityName?.Trim();
            if (!await DoesForecastingTaskEntityExist(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            return await _forecastingTasksRepository.GetForecastingTaskFieldsDeclaration(entityName);
        }

        public async Task<PagedForecastingTask> SearchForecastingTaskRecords(SearchForecastingTaskRecords searchRequest)
        {
            searchRequest.TaskEntityName = searchRequest.TaskEntityName?.Trim();
            if (searchRequest.PageNumber <= 0)
                throw new DomainErrorException("Page number should be greater than 0!");
            if (searchRequest.PerPage <= 0)
                throw new DomainErrorException("Per page amount should be greater than 0!");
            if (!await DoesForecastingTaskEntityExist(searchRequest.TaskEntityName))
                throw new DomainErrorException($"Forecasting task with name {searchRequest.TaskEntityName} doesn't exist!");

            foreach (var filter in searchRequest.Filters)
            {
                filter.Value = filter.Value?.Trim();
            }
            return await _forecastingTasksRepository.SearchForecastingTaskRecords(searchRequest);
        }

        private Task<bool> DoesForecastingTaskEntityExist(string entityName)
        {
            if (string.IsNullOrWhiteSpace(entityName))
                throw new DomainErrorException($"Forecasting task name must to be filled!");
            return _forecastingTasksRepository.DoesForecastingTaskEntityExist(entityName);
        }

        private string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(AppDomain.CurrentDomain.BaseDirectory);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            return Path.Combine(assemblyFolderPath, relativePath);
        }
        
        private void RemoveMLModelFiles(string entityName)
        {
            if (File.Exists(GetAbsolutePath($"preparation_{entityName}MLModel.zip")))
                File.Delete(GetAbsolutePath($"preparation_{entityName}MLModel.zip"));

            if (File.Exists(GetAbsolutePath($"{entityName}MLModel.zip")))
                File.Delete(GetAbsolutePath($"{entityName}MLModel.zip"));
        }

        private async Task ValidateCreateForecastingTaskEntity(string entityName, List<ForecastingTaskFieldDeclaration> declaration)
        {
            if (declaration.Count(x => x.Type == FieldType.PredictionField) != 1)
                throw new DomainErrorException($"Forecasting task must to have one predicated value!");

            if (declaration.Count(x => x.Type == FieldType.Factor) == 0)
                throw new DomainErrorException($"Forecasting task must to have at least 1 factor!");

            if (declaration.Any(x => string.IsNullOrEmpty(x.Name)))
                throw new DomainErrorException($"All fields 'name' must to be filled!");

            if (declaration.Select(x => x.Name.ToLower()).Distinct().Count() != declaration.Count)
                throw new DomainErrorException($"All fields 'name' must to be unique!");

            if (await DoesForecastingTaskEntityExist(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} already exists!");
        }
    }
}
