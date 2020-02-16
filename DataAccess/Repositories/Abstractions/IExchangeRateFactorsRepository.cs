using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Abstractions
{
    public interface IExchangeRateFactorsRepository
    {
        Task<ExchangeRateFactors> GetExchangeRateFactorsByDate(DateTime date);

        Task<List<ExchangeRateFactors>> GetExchangeRateFactorsRange(DateTime dateFrom, DateTime dateTo);

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
