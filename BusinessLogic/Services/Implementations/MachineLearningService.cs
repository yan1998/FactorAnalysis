using BusinessLogic.Exceptions;
using BusinessLogic.Services.Abstractions;
using DataAccess.Repositories.Abstractions;
using DomainModel.ForecastingTasks;
using FactorAnalysisML.Model;
using FactorAnalysisML.Model.ModelBuilders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Implementations
{
    public class MachineLearningService : IMachineLearningService
    {
        private readonly IForecastingTasksRepository _forecastingTasksRepository;

        public MachineLearningService(IForecastingTasksRepository forecastingTasksRepository)
        {
            _forecastingTasksRepository = forecastingTasksRepository;
        }


        public async Task CreateForecastingTaskMLModel(string entityName, LearningAlgorithm algorithm, bool isValidationNeeded = true)
        {
            entityName = entityName?.Trim();
            if (isValidationNeeded)
            {
                if (!await DoesForecastingTaskEntityExist(entityName))
                    throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");
            }

            try
            {
                var taskEntity = await _forecastingTasksRepository.GetForecastingTaskEntity(entityName);
                if (taskEntity.FieldsValues.Count == 0)
                    throw new DomainErrorException("There are no data in the database!");
                var nonInformationFields = taskEntity.FieldsDeclaration.Where(x => x.Type != FieldType.InformationField).ToList();
                var entity = new ClassBuilder(entityName, GetFieldsType(nonInformationFields));
                var dataList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(entity.Type));
                foreach (var fieldsValue in taskEntity.FieldsValues)
                {
                    var myClassInstance = entity.CreateObject();
                    foreach (var fieldDeclaration in nonInformationFields)
                    {
                        var value = fieldsValue.FieldsValue.Single(x => x.FieldId == fieldDeclaration.Id).Value;
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

        public async Task<float> PredictValueByFactors(string entityName, List<ForecastingTaskFieldValue> factors, bool isValidationNeeded = true)
        {
            entityName = entityName?.Trim();
            if (isValidationNeeded)
            {
                if (factors.Count == 0)
                    throw new DomainErrorException($"Factor list is empty!");
                if (!DoMLModelFilesExist(entityName))
                    throw new DomainErrorException("You didn't train the model! Please, do!");
                if (!await DoesForecastingTaskEntityExist(entityName))
                    throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");
            }
            var taskEntityDeclaration = await _forecastingTasksRepository.GetForecastingTaskFieldsDeclaration(entityName);
            var nonInformationFields = taskEntityDeclaration.Where(x => x.Type != FieldType.InformationField).ToList();
            var predictionValueId = taskEntityDeclaration.Single(x => x.Type == FieldType.PredictionField).Id;
            if (factors.Any(x => x.FieldId == predictionValueId))
                throw new DomainErrorException($"FieldId {predictionValueId} is incorrect! This is the prediction value!");

            var entity = new ClassBuilder(entityName, GetFieldsType(nonInformationFields));
            var myClassInstance = entity.CreateObject();
            foreach (var value in factors)
            {
                value.Value = value.Value?.Trim();
                if (!nonInformationFields.Any(x => x.Id == value.FieldId))
                    throw new DomainErrorException($"FieldId {value.FieldId} is incorrect!");

                var name = nonInformationFields.Single(x => x.Id == value.FieldId).Name;
                entity.SetPropertyValue(myClassInstance, name, float.Parse(value.Value));
            }

            var predicatedValue = ForecastingTaskConsumeModel.Predict(myClassInstance, entityName, entity.Type);
            return (float)Math.Round(predicatedValue, 3, MidpointRounding.AwayFromZero);
        }

        public async Task<List<AlgorithmPredictionReport>> AnalyzePredictionAlgorithms(string entityName, List<LearningAlgorithm> algorithms)
        {
            if (!await DoesForecastingTaskEntityExist(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            var report = new List<AlgorithmPredictionReport>();
            var data = await _forecastingTasksRepository.GetForecastingTaskEntity(entityName.Trim());
            var taskEntityDeclaration = data.FieldsDeclaration;
            var totalCount = data.FieldsValues.Count;
            var factorFieldIds = taskEntityDeclaration.Where(x => x.Type == FieldType.Factor).Select(x => x.Id).ToList();
            var predictionFieldId = taskEntityDeclaration.Single(x => x.Type == FieldType.PredictionField).Id;

            int tasksToSkip;
            if (totalCount <= 100)
                tasksToSkip = (int)Math.Floor(totalCount / 5.0);
            else if (totalCount > 100 && totalCount <= 500)
                tasksToSkip = (int)Math.Floor(totalCount / 10.0);
            else
                tasksToSkip = 30;

            foreach (var enumValue in algorithms)
            {
                var algorithmPredictionReportEntity = new AlgorithmPredictionReport { Algorithm = enumValue, Results = new List<AlgorithmPredictionResult>() };
                var stopwatch = new Stopwatch();

                stopwatch.Restart();
                CreateForecastingTaskMLModel(entityName, enumValue, false).GetAwaiter().GetResult();
                stopwatch.Stop();
                algorithmPredictionReportEntity.ElapsedTrainingTime = stopwatch.Elapsed;

                stopwatch.Restart();
                var iteration = 0;
                var errorSum = 0.0;
                do
                {
                    var fields = data.FieldsValues.Skip(iteration * tasksToSkip).Take(1).Single();
                    var predicationField = fields.FieldsValue.Single(x => x.FieldId == predictionFieldId);
                    var factorFields = fields.FieldsValue.Where(x => factorFieldIds.Contains(x.FieldId)).ToList();
                    var predicationResult = await PredictValueByFactors(data.Name, factorFields, false);
                    errorSum += predicationResult - float.Parse(predicationField.Value);

                    factorFields.Add(predicationField);
                    var algorithmPredictionResult = new AlgorithmPredictionResult
                    {
                        FactorValues = factorFields,
                        Result = predicationResult
                    };
                    algorithmPredictionReportEntity.Results.Add(algorithmPredictionResult);
                    iteration++;
                } while (iteration * tasksToSkip < data.FieldsValues.Count);

                stopwatch.Stop();
                algorithmPredictionReportEntity.ElapsedPredictionTime = stopwatch.Elapsed;
                algorithmPredictionReportEntity.AverageError = errorSum / (iteration + 1);
                report.Add(algorithmPredictionReportEntity);
            }

            return report;
        }

        private Task<bool> DoesForecastingTaskEntityExist(string entityName)
        {
            if (string.IsNullOrWhiteSpace(entityName))
                throw new DomainErrorException($"Forecasting task name must to be filled!");
            return _forecastingTasksRepository.DoesForecastingTaskEntityExist(entityName);
        }

        private bool DoMLModelFilesExist(string entityName)
        {
            return File.Exists(GetAbsolutePath($"preparation_{entityName}MLModel.zip")) && File.Exists(GetAbsolutePath($"{entityName}MLModel.zip"));
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

        private string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(AppDomain.CurrentDomain.BaseDirectory);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            return Path.Combine(assemblyFolderPath, relativePath);
        }
    }
}
