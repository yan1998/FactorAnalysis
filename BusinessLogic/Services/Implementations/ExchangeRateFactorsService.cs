using BusinessLogic.Models;
using BusinessLogic.Services.Abstractions;
using DataAccess.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactorAnalysisML.Model;
using AutoMapper;

namespace BusinessLogic.Services.Implementations
{
    public class ExchangeRateFactorsService : IExchangeRateFactorsService
    {
        private readonly IExchangeRateFactorsRepository _exchangeRateFactorsRepository;
        private readonly IMapper _mapper;

        public ExchangeRateFactorsService(IExchangeRateFactorsRepository exchangeRateFactorsRepository,
            IMapper mapper)
        {
            _exchangeRateFactorsRepository = exchangeRateFactorsRepository;
            _mapper = mapper;
        }

        public async Task<List<ExchangeRateFactors>> GetExchangeRateFactorsRange(DateTime dateFrom, DateTime dateTo)
        {
            if (dateFrom > dateTo)
                throw new Exception("DateTo cannot be less than dateFrom!");

            var exchangeRateFactors = await _exchangeRateFactorsRepository.GetExchangeRateFactorsRange(dateFrom, dateTo);
            return _mapper.Map<List<ExchangeRateFactors>>(exchangeRateFactors);
        }

        public float PredicateUSDCurrencyExchange(ExchangeRateFactors factors)
        {
            ModelInput input = new ModelInput
            {
                CreditRate = factors.CreditRate,
                GDPIndicator = factors.GDPIndicator,
                ExportIndicator = factors.ExportIndicator,
                ImportIndicator = factors.ImportIndicator,
                InflationIndex = factors.InflationIndex
            };

            return ConsumeModel.Predict(input).Score;
        }
    }
}
