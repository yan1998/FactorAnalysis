using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DomainModel.ExchangeRateFactors;

namespace BusinessLogic.Services.Abstractions
{
    public interface IExchangeRateFactorsService
    {
        Task<List<ExchangeRateFactors>> GetExchangeRateFactorsRange(DateTime dateFrom, DateTime dateTo);

        float PredictUSDCurrencyExchange(ExchangeRateFactors factors);

        float PredictEURCurrencyExchange(ExchangeRateFactors factors);

        Task<ExchangeRateFactors> GetExchangeRateFactorsById(int id);

        Task<PagedExchangeRateFactors> GetPagedExchangeRateFactors(int pageNumber, int perPage);

        Task CreateExchangeRateFactors(ExchangeRateFactors factors);

        Task UpdateExchangeRateFactors(ExchangeRateFactors factors);

        Task RemoveExchangeRateFactors(int id);
    }
}
