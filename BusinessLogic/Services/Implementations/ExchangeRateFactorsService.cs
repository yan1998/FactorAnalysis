using BusinessLogic.Models;
using BusinessLogic.Services.Abstractions;
using DataAccess.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactorAnalysisML.Model;

namespace BusinessLogic.Services.Implementations
{
    public class ExchangeRateFactorsService : IExchangeRateFactorsService
    {
        private readonly IExchangeRateFactorsRepository _exchangeRateFactorsRepository;

        public ExchangeRateFactorsService(IExchangeRateFactorsRepository exchangeRateFactorsRepository)
        {
            _exchangeRateFactorsRepository = exchangeRateFactorsRepository;
        }

        public async Task<List<ExchangeRateFactors>> GetExchangeRateFactorsRange(DateTime dateFrom, DateTime dateTo)
        {
            if (dateFrom > dateTo)
                throw new Exception("DateTo cannot be less than dateFrom!");

            var exchangeRateFactors = await _exchangeRateFactorsRepository.GetExchangeRateFactorsRange(dateFrom, dateTo);
            //Should be refactored with AutoMapper
            return exchangeRateFactors.Select(x => new ExchangeRateFactors
            {
                CreditRate = x.CreditRate,
                Date = x.Date,
                ExchangeRateEUR = x.ExchangeRateEUR,
                ExchangeRateUSD = x.ExchangeRateUSD,
                ExportIndicator = x.ExportIndicator,
                GDPIndicator = x.GDPIndicator,
                Id = x.Id,
                ImportIndicator = x.ImportIndicator,
                InflationIndex = x.InflationIndex
            }).ToList();
        }

        public float PredicateUSDCurrencyExchange(ExchangeRateFactors factors)
        {
            ModelInput input = new ModelInput
            {
                CreditRate = (float)factors.CreditRate,
                GDPIndicator = factors.GDPIndicator,
                ExportIndicator = (float)factors.ExportIndicator,
                ImportIndicator = (float)factors.ImportIndicator,
                InflationIndex = (float)factors.InflationIndex
            };

            return ConsumeModel.Predict(input).Score;
        }
    }
}
