﻿using BusinessLogic.Exceptions;
using BusinessLogic.Services.Abstractions;
using DataAccess.Repositories.Abstractions;
using DomainModel.ForecastingTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Implementations
{
    public class ImportExportInFileService : IImportExportInFileService
    {
        private readonly IForecastingTasksRepository _forecastingTasksRepository;

        public ImportExportInFileService(IForecastingTasksRepository forecastingTasksRepository)
        {
            _forecastingTasksRepository = forecastingTasksRepository;
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

        public async Task<string> GenerateCsvString(string entityName)
        {
            entityName = entityName?.Trim();
            if (!await DoesForecastingTaskEntityExist(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            try
            {
                var taskEntity = await _forecastingTasksRepository.GetForecastingTaskEntity(entityName);
                var result = new StringBuilder(string.Join(',', taskEntity.FieldsDeclaration.Select(x => x.Name)));

                foreach (var fieldsValue in taskEntity.FieldsValues)
                {
                    var tempStr = "\r\n";
                    foreach (var factorDeclaration in taskEntity.FieldsDeclaration)
                    {
                        var value = fieldsValue.FieldsValue.Single(x => x.FieldId == factorDeclaration.Id).Value;
                        tempStr += value + ',';
                    }
                    result.Append(tempStr[0..^1]);
                }

                return result.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Task<bool> DoesForecastingTaskEntityExist(string entityName)
        {
            if (string.IsNullOrWhiteSpace(entityName))
                throw new DomainErrorException($"Forecasting task name must to be filled!");
            return _forecastingTasksRepository.DoesForecastingTaskEntityExist(entityName);
        }
    }
}
