using BusinessLogic.Exceptions;
using BusinessLogic.Services.Abstractions;
using DataAccess.Repositories.Abstractions;
using DomainModel.ForecastingTasks;
using FactorAnalysisML.Model.ModelBuilders;
using System;
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

        public async Task CreateForecastingTaskEntity(string entityName, List<ForecastingTaskFactorDeclaration> declaration)
        {
            if(declaration.Count(x => x.IsPredicatedValue) != 1)
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
            if(!await DoesForecastingTaskEntityExists(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            await _forecastingTasksRepository.DeleteForecastingTaskEntity(entityName);
        }

        public async Task AddForecastingTaskFactors(string entityName, List<ForecastingTaskFactorValue> values)
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
            if(string.IsNullOrWhiteSpace(entityName))
                throw new DomainErrorException($"Forecasting task name must to be filled!");
            if (!await DoesForecastingTaskEntityExists(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            return await _forecastingTasksRepository.GetPagedForecastingTaskEntity(entityName, pageNumber, perPage);
        }

        //public async Task<List<object>> GetForecastingTaskEntityForCsv(string entityName)
        //{
        //    if (string.IsNullOrWhiteSpace(entityName))
        //        throw new DomainErrorException($"Forecasting task name must to be filled!");

        //    try
        //    {
        //        var taskEntity = await _forecastingTasksRepository.GetForecastingTaskEntity(entityName);
        //        var propertyNames = taskEntity.FactorsDeclaration.Select(x => x.Name).ToList();
        //        var entity = new ClassBuilder(entityName, propertyNames, typeof(float));
        //        var dataList = new List<object>();
        //        foreach (var factorsValue in taskEntity.FactorsValues)
        //        {
        //            var myClassInstance = entity.CreateObject();
        //            foreach (var factorDeclaration in taskEntity.FactorsDeclaration)
        //            {
        //                var value = factorsValue.FactorsValue.Single(x => x.FactorId == factorDeclaration.Id).Value;
        //                entity.SetPropertyValue(myClassInstance, factorDeclaration.Name, value);
        //            }
        //            dataList.Add(myClassInstance);
        //        }

        //        return dataList;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task<string> GetForecastingTaskEntityForCsv(string entityName)
        {
            if (!await DoesForecastingTaskEntityExists(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            try
            {
                var taskEntity = await _forecastingTasksRepository.GetForecastingTaskEntity(entityName);
                var result = string.Join(',', taskEntity.FactorsDeclaration.Select(x => x.Name));
                foreach (var factorsValue in taskEntity.FactorsValues)
                {
                    var tempStr = "\r\n";
                    foreach (var factorDeclaration in taskEntity.FactorsDeclaration)
                    {
                        var value = factorsValue.FactorsValue.Single(x => x.FactorId == factorDeclaration.Id).Value;
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

            var taskEntityDeclaration = await _forecastingTasksRepository.GetForecastingTaskFactorDeclaration(entityName);
            var rows = csv.Split("\r\n");
            var factorsOrder = new Dictionary<int, int>();

            // Checking csv header
            var headerColumns = rows.First().Split(',');
            if(taskEntityDeclaration.Count != headerColumns.Count())
                throw new DomainErrorException($"Forecasting task with name {entityName} and csv file have a different count of columns!");

            for(int i = 0; i < headerColumns.Length; i++)
            {
                if(!taskEntityDeclaration.Any(x => x.Name == headerColumns[i]))
                    throw new DomainErrorException($"Column {headerColumns[i]} doesn't exist in forecasting task with name {entityName}!");

                factorsOrder.Add(i, taskEntityDeclaration.Single(x => x.Name == headerColumns[i]).Id);
            }

            //Add factors values
            foreach (var row in rows.Skip(1))
            {
                var factorsValue = new List<ForecastingTaskFactorValue>();
                var columns = row.Split(',');
                for (int i = 0; i < columns.Length; i++)
                {
                    factorsValue.Add(new ForecastingTaskFactorValue
                    {
                        FactorId = factorsOrder[i],
                        Value = float.Parse(columns[i])
                    });
                }
                await _forecastingTasksRepository.AddForecastingTaskFactors(entityName, factorsValue);
            }
        }

        public async Task CreateForecastingTaskMLModel(string entityName)
        {
            if (string.IsNullOrWhiteSpace(entityName))
                throw new DomainErrorException($"Forecasting task name must to be filled!");

            try
            {
                var taskEntity = await _forecastingTasksRepository.GetForecastingTaskEntity(entityName);
                var propertyNames = taskEntity.FactorsDeclaration.Select(x => x.Name).ToList();
                var entity = new ClassBuilder(entityName, propertyNames, typeof(float));
                var dataList = new List<object>();
                // var dataList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(entity.CreateObject().GetType()));

                foreach (var factorsValue in taskEntity.FactorsValues)
                {
                    var myClassInstance = entity.CreateObject();
                    foreach (var factorDeclaration in taskEntity.FactorsDeclaration)
                    {
                        var value = factorsValue.FactorsValue.Single(x => x.FactorId == factorDeclaration.Id).Value;
                        entity.SetPropertyValue(myClassInstance, factorDeclaration.Name, value);
                    }
                    dataList.Add(myClassInstance);
                }

                var factors = taskEntity.FactorsDeclaration.Where(x => !x.IsPredicatedValue).Select(x => x.Name);
                var predictedValue = taskEntity.FactorsDeclaration.Single(x => x.IsPredicatedValue).Name;
                FactoryTaskModelBuilder.CreateModel(dataList, entityName, factors, predictedValue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<bool> DoesForecastingTaskEntityExists(string entityName)
        {
            var existingEntityNames = await _forecastingTasksRepository.GetAllForecastingTaskEntitiesName();
            return existingEntityNames.Contains(entityName);
        }
    }
}
