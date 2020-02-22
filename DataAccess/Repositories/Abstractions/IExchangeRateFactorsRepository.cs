using DomainModel.ExchangeRateFactors;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Abstractions
{
    public interface IExchangeRateFactorsRepository
    {
        Task<ExchangeRateFactors> GetExchangeRateFactorsByDate(DateTime date);

        Task<List<ExchangeRateFactors>> GetExchangeRateFactorsRange(DateTime dateFrom, DateTime dateTo);

        Task<ExchangeRateFactors> GetExchangeRateFactorsById(int id);

        Task<PagedExchangeRateFactors> GetPagedExchangeRateFactors(int pageNumber, int perPage);

        Task CreateExchangeRateFactors(ExchangeRateFactors factors);

        Task UpdateExchangeRateFactors(ExchangeRateFactors factors);

        Task RemoveExchangeRateFactors(int id);

        Task<bool> DoesExchangeRateFactorsExist(DateTime date);

        #region SeedData

        Task AddOrUpdateExchangeRateUSD(DateTime date, decimal exchangeRateUSD);

        Task AddOrUpdateExchangeRateEUR(DateTime date, decimal exchangeRateEUR);

        Task AddOrUpdateCreditRate(DateTime date, float creditRate);

        Task AddOrUpdateGDPIndicator(DateTime date, long gdpIndicator);

        Task AddOrUpdateImportIndicator(DateTime date, float importIndicator);

        Task AddOrUpdateExportIndicator(DateTime date, float exportIndicator);

        Task AddOrUpdateInflationIndex(DateTime date, float inflationIndex);

        #endregion
    }
}
