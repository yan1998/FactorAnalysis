﻿using BusinessLogic.Models;
using BusinessLogic.Services.Abstractions;
using CsvHelper;
using DataAccess.Repositories.Abstractions;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Implementations
{
    public class SeedExchangeRateFactorsService : ISeedExchangeRateFactorsService
    {
        private readonly IExchangeRateFactorsRepository _exchangeRateFactorsRepository;

        public SeedExchangeRateFactorsService(IExchangeRateFactorsRepository exchangeRateFactorsRepository)
        {
            _exchangeRateFactorsRepository = exchangeRateFactorsRepository;
        }

        public async Task FillCreditRate(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var csvReader = new CsvReader(reader);
                var records = csvReader.GetRecords<SeedFileDataRange<double>>().Where(x => x.DateFrom.Year >= 2000);
                foreach (var record in records)
                {
                    var tempDate = record.DateFrom;
                    while (tempDate.Date <= record.DateTo.Date)
                    {
                        await _exchangeRateFactorsRepository.AddOrUpdateCreditRate(tempDate.Date, record.Value);
                        tempDate = tempDate.AddDays(1);
                    }
                }
            }
        }

        public async Task FillExchangeRateEUR(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var csvReader = new CsvReader(reader);
                var records = csvReader.GetRecords<SeedFileData<decimal>>();
                foreach (var record in records)
                {
                   await _exchangeRateFactorsRepository.AddOrUpdateExchangeRateEUR(record.Date, record.Value);
                }
            }
        }

        public async Task FillExchangeRateUSD(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var csvReader = new CsvReader(reader);
                var records = csvReader.GetRecords<SeedFileData<decimal>>();
                foreach (var record in records)
                {
                    await _exchangeRateFactorsRepository.AddOrUpdateExchangeRateUSD(record.Date, record.Value);
                }
            }
        }

        public async Task FillExportIndicator(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var csvReader = new CsvReader(reader);
                var records = csvReader.GetRecords<SeedFileData<double>>();
                foreach (var record in records)
                {
                    var dateFrom = record.Date;
                    var dateTo = record.Date.AddMonths(1);
                    while (dateFrom.Date <= dateTo.Date)
                    {
                        await _exchangeRateFactorsRepository.AddOrUpdateExportIndicator(dateFrom, record.Value);
                        dateFrom = dateFrom.AddDays(1);
                    }
                }
            }
        }

        public Task FillGDPIndicator(DateTime date, long gdpIndicator)
        {
            return _exchangeRateFactorsRepository.AddOrUpdateGDPIndicator(date, gdpIndicator);
        }

        public async Task FillImportIndicator(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var csvReader = new CsvReader(reader);
                var records = csvReader.GetRecords<SeedFileData<double>>();
                foreach (var record in records)
                {
                    var dateFrom = record.Date;
                    var dateTo = record.Date.AddMonths(1);
                    while (dateFrom.Date <= dateTo.Date)
                    {
                        await _exchangeRateFactorsRepository.AddOrUpdateImportIndicator(dateFrom, record.Value);
                        dateFrom = dateFrom.AddDays(1);
                    }
                }
            }
        }

        public Task FillInflationIndex(DateTime date, double inflationIndex)
        {
            return _exchangeRateFactorsRepository.AddOrUpdateInflationIndex(date, inflationIndex);
        }
    }
}
