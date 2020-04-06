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

        public Task<List<string>> GetAllForecastingTaskEntitiesName()
        {
            return _forecastingTasksRepository.GetAllForecastingTaskEntitiesName();
        }

        public async Task CreateForecastingTaskEntity(string entityName, List<ForecastingTaskFieldDeclaration> declaration)
        {
            if (declaration.Count(x => x.Type == FieldType.PredictionField) != 1)
                throw new DomainErrorException($"Forecasting task must to have one predicated value!");

            if (await DoesForecastingTaskEntityExists(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} already exists!");

            int i = 0;
            foreach (var declarationItem in declaration)
            {
                declarationItem.Id = i++;
            }
            await _forecastingTasksRepository.CreateForecastingTaskEntity(entityName, declaration);
        }

        public async Task DeleteForecastingTaskEntity(string entityName)
        {
            if (!await DoesForecastingTaskEntityExists(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            await _forecastingTasksRepository.DeleteForecastingTaskEntity(entityName);

            if (File.Exists(GetAbsolutePath($"preparation_{entityName}MLModel.zip")))
                File.Delete(GetAbsolutePath($"preparation_{entityName}MLModel.zip"));

            if (File.Exists(GetAbsolutePath($"{entityName}MLModel.zip")))
                File.Delete(GetAbsolutePath($"{entityName}MLModel.zip"));
        }

        public async Task AddForecastingTaskFactors(string entityName, List<ForecastingTaskFieldValue> values)
        {
            if (!await DoesForecastingTaskEntityExists(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            await _forecastingTasksRepository.AddForecastingTaskFactors(entityName, values);
        }

        public async Task DeleteForecastingTaskFactorsById(string entityName, string id)
        {
            if (!await DoesForecastingTaskEntityExists(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            await _forecastingTasksRepository.DeleteForecastingTaskFactorsById(entityName, id);
        }

        public async Task<PagedForecastingTask> GetPagedForecastingTaskEntity(string entityName, int pageNumber, int perPage)
        {
            if (pageNumber <= 0)
                throw new DomainErrorException("Page number should be greater than 0!");
            if (perPage <= 0)
                throw new DomainErrorException("Per page amount should be greater than 0!");
            if (!await DoesForecastingTaskEntityExists(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            return await _forecastingTasksRepository.GetPagedForecastingTaskEntity(entityName, pageNumber, perPage);
        }

        public async Task<string> GetForecastingTaskEntityForCsv(string entityName)
        {
            if (!await DoesForecastingTaskEntityExists(entityName))
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
                        tempStr += value.ToString() + ',';
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
            if (!await DoesForecastingTaskEntityExists(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            var taskEntityDeclaration = await _forecastingTasksRepository.GetForecastingTaskFieldDeclaration(entityName);
            var rows = csv.Split("\r\n");
            var factorsOrder = new Dictionary<int, int>();

            // Checking csv header
            var headerColumns = rows.First().Split(',');
            if (taskEntityDeclaration.Count != headerColumns.Count())
                throw new DomainErrorException($"Forecasting task with name {entityName} and csv file have a different count of columns!");

            for (int i = 0; i < headerColumns.Length; i++)
            {
                if (!taskEntityDeclaration.Any(x => x.Name == headerColumns[i]))
                    throw new DomainErrorException($"Column {headerColumns[i]} doesn't exist in forecasting task with name {entityName}!");

                factorsOrder.Add(i, taskEntityDeclaration.Single(x => x.Name == headerColumns[i]).Id);
            }

            //Add factors values
            foreach (var row in rows.Skip(1))
            {
                var factorsValue = new List<ForecastingTaskFieldValue>();
                var columns = row.Split(',');
                for (int i = 0; i < columns.Length; i++)
                {
                    factorsValue.Add(new ForecastingTaskFieldValue
                    {
                        FieldId = factorsOrder[i],
                        Value = float.Parse(columns[i])
                    });
                }
                await _forecastingTasksRepository.AddForecastingTaskFactors(entityName, factorsValue);
            }
        }

        public async Task CreateForecastingTaskMLModel(string entityName)
        {
            if (!await DoesForecastingTaskEntityExists(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            try
            {
                var taskEntity = await _forecastingTasksRepository.GetForecastingTaskEntity(entityName);
                var propertyNames = taskEntity.FieldsDeclaration.Select(x => x.Name).ToList();
                var entity = new ClassBuilder(entityName, propertyNames, typeof(float));
                var dataList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(entity.Type));
                foreach (var fieldsValue in taskEntity.FieldsValues)
                {
                    var myClassInstance = entity.CreateObject();
                    foreach (var factorDeclaration in taskEntity.FieldsDeclaration)
                    {
                        var value = fieldsValue.FieldsValue.Single(x => x.FieldId == factorDeclaration.Id).Value;
                        entity.SetPropertyValue(myClassInstance, factorDeclaration.Name, value);
                    }
                    dataList.Add(myClassInstance);
                }

                var factors = taskEntity.FieldsDeclaration.Where(x => x.Type != FieldType.PredictionField).Select(x => x.Name);
                var predictedValue = taskEntity.FieldsDeclaration.Single(x => x.Type == FieldType.PredictionField).Name;
                ForecastingTaskModelBuilder.CreateModel(dataList, entityName, factors, predictedValue);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<float> PredictValue(string entityName, List<ForecastingTaskFieldValue> values)
        {
            // validation block
            if (!await DoesForecastingTaskEntityExists(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");
            var taskEntityDeclaration = await _forecastingTasksRepository.GetForecastingTaskFieldDeclaration(entityName);
            var predictionValueId = taskEntityDeclaration.Single(x => x.Type == FieldType.PredictionField).Id;
            if (values.Any(x => x.FieldId == predictionValueId))
                throw new DomainErrorException($"FieldId {predictionValueId} is incorrect! This is the prediction value!");

            var propertyNames = taskEntityDeclaration.Select(x => x.Name).ToList();
            var entity = new ClassBuilder(entityName, propertyNames, typeof(float));
            var myClassInstance = entity.CreateObject();
            foreach (var value in values)
            {
                if (!taskEntityDeclaration.Any(x => x.Id == value.FieldId))
                    throw new DomainErrorException($"FieldId {value.FieldId} is incorrect!");

                var name = taskEntityDeclaration.Single(x => x.Id == value.FieldId).Name;
                entity.SetPropertyValue(myClassInstance, name, value.Value);
            }

            return ForecastingTaskConsumeModel.Predict(myClassInstance, entityName, entity.Type);
        }

        private async Task<bool> DoesForecastingTaskEntityExists(string entityName)
        {
            if (string.IsNullOrWhiteSpace(entityName))
                throw new DomainErrorException($"Forecasting task name must to be filled!");
            var existingEntityNames = await _forecastingTasksRepository.GetAllForecastingTaskEntitiesName();
            return existingEntityNames.Contains(entityName);
        }


        private static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(AppDomain.CurrentDomain.BaseDirectory);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            return Path.Combine(assemblyFolderPath, relativePath);
        }
    }
}
