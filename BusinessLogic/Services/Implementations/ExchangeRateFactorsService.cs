using BusinessLogic.Services.Abstractions;
using DataAccess.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FactorAnalysisML.Model;
using AutoMapper;
using FactorAnalysisML.Model.Models;
using BusinessLogic.Exceptions;
using DomainModel.ExchangeRateFactors;
using FactorAnalysisML.Model.ModelBuilders;

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

            return await _exchangeRateFactorsRepository.GetExchangeRateFactorsRange(dateFrom, dateTo);
        }

        public float PredictUSDCurrencyExchange(ExchangeRateFactors factors)
        {
            var input = _mapper.Map<CurrencyExchangeModelInput>(factors);
            return USDCurrencyExchangeConsumeModel.Predict(input).Score;
        }

        public float PredictEURCurrencyExchange(ExchangeRateFactors factors)
        {
            var input = _mapper.Map<CurrencyExchangeModelInput>(factors);
            return EURCurrencyExchangeConsumeModel.Predict(input).Score;
        }

        public async Task<ExchangeRateFactors> GetExchangeRateFactorsById(int id)
        {
            if (id <= 0)
                throw new DomainErrorException("Id should be greater than 0!");

            return await _exchangeRateFactorsRepository.GetExchangeRateFactorsById(id);
        }

        public async Task<PagedExchangeRateFactors> GetPagedExchangeRateFactors(int pageNumber, int perPage)
        {
            if (pageNumber <= 0)
                throw new DomainErrorException("Page number should be greater than 0!");
            if (perPage <= 0)
                throw new DomainErrorException("Per page amount should be greater than 0!");

            return await _exchangeRateFactorsRepository.GetPagedExchangeRateFactors(pageNumber, perPage);
        }

        public async Task CreateExchangeRateFactors(ExchangeRateFactors factors)
        {
            ValidateExchangeRateFactors(factors);
            var doesExist = await _exchangeRateFactorsRepository.DoesExchangeRateFactorsExist(factors.Date);
            if (doesExist)
                throw new DomainErrorException($"Exchange rate factors with date = {factors.Date.ToString("d")} exists in db");

            await _exchangeRateFactorsRepository.CreateExchangeRateFactors(factors);
        }

        public async Task UpdateExchangeRateFactors(ExchangeRateFactors factors)
        {
            ValidateExchangeRateFactors(factors);
            var doesExist = await _exchangeRateFactorsRepository.DoesExchangeRateFactorsExist(factors.Date);
            if (!doesExist)
                throw new DomainErrorException($"Exchange rate factors with date = {factors.Date.ToString("d")} doesn't exist in db");

            await _exchangeRateFactorsRepository.UpdateExchangeRateFactors(factors);
        }

        public async Task RemoveExchangeRateFactors(int id)
        {
            if (id <= 0)
            {
                throw new DomainErrorException("Id should be greater than 0!");
            }
            await _exchangeRateFactorsRepository.RemoveExchangeRateFactors(id);
        }

        public async Task CreateEURCurrencyExchangeMLModel()
        {
            var data = await _exchangeRateFactorsRepository.GetExchangeRateFactorsRange(DateTime.MinValue, DateTime.MaxValue);
            EURCurrencyExchangeModelBuilder.CreateModel(_mapper.Map<List<CurrencyExchangeModelInput>>(data));
        }

        public Task RetrainEURCurrencyExchangeMLModel()
        {
            EURCurrencyExchangeModelBuilder.RetrainModel(null);
            return Task.CompletedTask;
        }

        private void ValidateExchangeRateFactors(ExchangeRateFactors factors)
        {
            if (factors.Date.Year < 1991 || factors.Date.Date > DateTime.Now)
                throw new DomainErrorException($"Date should be greater than 1991-01-01 and less than {DateTime.Now.ToString("d")}");
            if (factors.ExchangeRateUSD < 0)
                throw new DomainErrorException("ExchangeRateUSD should be greater than 0");
            if (factors.ExchangeRateEUR < 0)
                throw new DomainErrorException("ExchangeRateEUR should be greater than 0");
            if (factors.CreditRate < 0)
                throw new DomainErrorException("CreditRate should be greater than 0");
            if (factors.ExportIndicator < 0)
                throw new DomainErrorException("ExportIndicator should be greater than 0");
            if (factors.ImportIndicator < 0)
                throw new DomainErrorException("ImportIndicator should be greater than 0");
            if (factors.InflationIndex < 0)
                throw new DomainErrorException("InflationIndex should be greater than 0");
            if (factors.GDPIndicator < 0)
                throw new DomainErrorException("GDPIndicator should be greater than 0");
        }
    }
}
