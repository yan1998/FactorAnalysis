using BusinessLogic.Exceptions;
using BusinessLogic.Services.Abstractions;
using DataAccess.Repositories.Abstractions;
using DomainModel.ForecastingTasks;
using FactorAnalysisML.Model;
using FactorAnalysisML.Model.ModelBuilders;
using System;
using System.IO;
using System.Collections;
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
            await CreateForecastingTaskEntityValidation(entityName, declaration);

            int i = 0;
            foreach (var declarationItem in declaration)
            {
                declarationItem.Id = i++;
                declarationItem.Name = declarationItem.Name?.Trim();
                declarationItem.Description = declarationItem.Description?.Trim();
            }
            await _forecastingTasksRepository.CreateForecastingTaskEntity(entityName, description, declaration);
        }

        public async Task RenameForecastingTaskEntity(string oldTaskName, string newTaskName, string newTaskDescription)
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

        public async Task AddForecastingTaskFactors(string entityName, List<ForecastingTaskFieldValue> values)
        {
            entityName = entityName?.Trim();
            if (!await DoesForecastingTaskEntityExist(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            var taskEntityDeclaration = await _forecastingTasksRepository.GetForecastingTaskFieldsDeclaration(entityName);
            if (taskEntityDeclaration.Count != values.Select(x => x.FieldId).Distinct().Count())
                throw new DomainErrorException($"Forecasting task with name {entityName} and the request have a different count of fields!");

            for (int i = 0; i < values.Count; i++)
            {
                if (!taskEntityDeclaration.Any(x => x.Id == values[i].FieldId))
                    throw new DomainErrorException($"Column {values[i].FieldId} doesn't exist in forecasting task with name {entityName}!");

                values[i].Value = values[i].Value?.Trim();
                var fieldDeclaration = taskEntityDeclaration.Single(x => x.Id == values[i].FieldId);
                if (fieldDeclaration.Type != FieldType.InformationField && !float.TryParse(values[i].Value, out _))
                    throw new DomainErrorException($"Field {fieldDeclaration.Name} must to be filled with a number! But was filled with value: {values[i].Value}");
            }

            await _forecastingTasksRepository.AddForecastingTaskFields(entityName, values);
        }

        public async Task DeleteForecastingTaskFactorsById(string entityName, string id)
        {
            entityName = entityName?.Trim();
            if (!await DoesForecastingTaskEntityExist(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            await _forecastingTasksRepository.DeleteForecastingTaskFieldsById(entityName, id);
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

        public async Task<string> GetForecastingTaskEntityForCsv(string entityName)
        {
            entityName = entityName?.Trim();
            if (!await DoesForecastingTaskEntityExist(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            try
            {
                var taskEntity = await _forecastingTasksRepository.GetForecastingTaskEntity(entityName);
                var result = string.Join(',', taskEntity.FieldsDeclaration.Select(x => x.Name));
                foreach (var fieldsValue in taskEntity.FieldsValues)
                {
                    var tempStr = "\r\n";
                    foreach (var factorDeclaration in taskEntity.FieldsDeclaration)
                    {
                        var value = fieldsValue.FieldsValue.Single(x => x.FieldId == factorDeclaration.Id).Value;
                        tempStr += value + ',';
                    }
                    result += tempStr[0..^1];
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddForecastingTaskFactorsViaCsv(string entityName, string csv)
        {
            entityName = entityName?.Trim();
            if (!await DoesForecastingTaskEntityExist(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            var taskEntityDeclaration = await _forecastingTasksRepository.GetForecastingTaskFieldsDeclaration(entityName);
            var rows = csv.Split("\r\n");
            var fieldsOrder = new Dictionary<int, ForecastingTaskFieldDeclaration>();

            // Checking csv header
            var headerColumns = rows.First().Split(',');
            if (taskEntityDeclaration.Count != headerColumns.Count())
                throw new DomainErrorException($"Forecasting task with name {entityName} and csv file have a different count of columns!");

            for (int i = 0; i < headerColumns.Length; i++)
            {
                if (!taskEntityDeclaration.Any(x => x.Name == headerColumns[i]))
                    throw new DomainErrorException($"Column {headerColumns[i]} doesn't exist in forecasting task with name {entityName}!");

                fieldsOrder.Add(i, taskEntityDeclaration.Single(x => x.Name == headerColumns[i]));
            }

            //Add fields values
            var fieldsValues = new List<List<ForecastingTaskFieldValue>>();
            foreach (var row in rows.Skip(1))
            {
                var factorsValue = new List<ForecastingTaskFieldValue>();
                var columns = row.Split(',');
                for (int i = 0; i < columns.Length; i++)
                {
                    columns[i] = columns[i]?.Trim();
                    if (fieldsOrder[i].Type != FieldType.InformationField && !float.TryParse(columns[i], out _))
                        throw new DomainErrorException($"Field {fieldsOrder[i].Name} must to be filled with a number! But was filled with value: {columns[i]}");

                    factorsValue.Add(new ForecastingTaskFieldValue
                    {
                        FieldId = fieldsOrder[i].Id,
                        Value = columns[i]
                    });
                }
                fieldsValues.Add(factorsValue);
            }

            await _forecastingTasksRepository.AddBatchOfForecastingTaskFields(entityName, fieldsValues);
        }

        public async Task CreateForecastingTaskMLModel(string entityName, LearningAlgorithm algorithm)
        {
            entityName = entityName?.Trim();
            if (!await DoesForecastingTaskEntityExist(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            try
            {
                var taskEntity = await _forecastingTasksRepository.GetForecastingTaskEntity(entityName);
                var nonInformationFields = taskEntity.FieldsDeclaration.Where(x => x.Type != FieldType.InformationField).ToList();
                var entity = new ClassBuilder(entityName, GetFieldsType(nonInformationFields));
                var dataList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(entity.Type));
                foreach (var fieldsValue in taskEntity.FieldsValues)
                {
                    var myClassInstance = entity.CreateObject();
                    foreach (var fieldDeclaration in nonInformationFields)
                    {
                        var value= fieldsValue.FieldsValue.Single(x => x.FieldId == fieldDeclaration.Id).Value;
                        entity.SetPropertyValue(myClassInstance, fieldDeclaration.Name, float.Parse(value));
                    }
                    dataList.Add(myClassInstance);
                }

                var factors = nonInformationFields.Where(x => x.Type == FieldType.Factor).Select(x => x.Name);
                var predictedValue = nonInformationFields.Single(x => x.Type == FieldType.PredictionField).Name;
                ForecastingTaskModelBuilder.CreateModel(dataList, entityName, factors, predictedValue, algorithm);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<float> PredictValue(string entityName, List<ForecastingTaskFieldValue> values)
        {
            entityName = entityName?.Trim();
            if (values.Count == 0)
                throw new DomainErrorException($"Factor list is empty!");
            if (!DoMLModelFilesExist(entityName))
                throw new DomainErrorException("You didn't train the model! Please, do!");
            if (!await DoesForecastingTaskEntityExist(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");
            var taskEntityDeclaration = await _forecastingTasksRepository.GetForecastingTaskFieldsDeclaration(entityName);
            var nonInformationFields = taskEntityDeclaration.Where(x => x.Type != FieldType.InformationField).ToList();
            var predictionValueId = taskEntityDeclaration.Single(x => x.Type == FieldType.PredictionField).Id;
            if (values.Any(x => x.FieldId == predictionValueId))
                throw new DomainErrorException($"FieldId {predictionValueId} is incorrect! This is the prediction value!");

            var entity = new ClassBuilder(entityName, GetFieldsType(nonInformationFields));
            var myClassInstance = entity.CreateObject();
            foreach (var value in values)
            {
                value.Value = value.Value?.Trim();
                if (!nonInformationFields.Any(x => x.Id == value.FieldId))
                    throw new DomainErrorException($"FieldId {value.FieldId} is incorrect!");

                var name = nonInformationFields.Single(x => x.Id == value.FieldId).Name;
                entity.SetPropertyValue(myClassInstance, name, float.Parse(value.Value));
            }

            return ForecastingTaskConsumeModel.Predict(myClassInstance, entityName, entity.Type);
        }

        private async Task<bool> DoesForecastingTaskEntityExist(string entityName)
        {
            if (string.IsNullOrWhiteSpace(entityName))
                throw new DomainErrorException($"Forecasting task name must to be filled!");
            var existingEntityNames = await _forecastingTasksRepository.GetAllForecastingTaskEntities();
            return existingEntityNames.Any(x => x.Name.ToLower() == entityName.ToLower());
        }

        private string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(AppDomain.CurrentDomain.BaseDirectory);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            return Path.Combine(assemblyFolderPath, relativePath);
        }


        private bool DoMLModelFilesExist(string entityName)
        {
            return File.Exists(GetAbsolutePath($"preparation_{entityName}MLModel.zip")) && File.Exists(GetAbsolutePath($"{entityName}MLModel.zip"));
        }
        
        private void RemoveMLModelFiles(string entityName)
        {
            if (File.Exists(GetAbsolutePath($"preparation_{entityName}MLModel.zip")))
                File.Delete(GetAbsolutePath($"preparation_{entityName}MLModel.zip"));

            if (File.Exists(GetAbsolutePath($"{entityName}MLModel.zip")))
                File.Delete(GetAbsolutePath($"{entityName}MLModel.zip"));
        }

        private Dictionary<string, Type> GetFieldsType(List<ForecastingTaskFieldDeclaration> fieldsDeclaration)
        {
            var result = new Dictionary<string, Type>();
            foreach (var fieldDeclaration in fieldsDeclaration)
            {
                var type = fieldDeclaration.Type == FieldType.InformationField ? typeof(string) : typeof(float); 
                result.Add(fieldDeclaration.Name, type);
            }
            return result;
        }

        private async Task CreateForecastingTaskEntityValidation(string entityName, List<ForecastingTaskFieldDeclaration> declaration)
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
