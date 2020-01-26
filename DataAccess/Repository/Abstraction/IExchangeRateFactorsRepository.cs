using DataAccess.Model;
using System;
using System.Threading.Tasks;

namespace DataAccess.Repository.Abstraction
{
    public interface IExchangeRateFactorsRepository
    {
        Task<ExchangeRateFactors> GetExchangeRateFactorsByDate(DateTime date);

        #region SeedData

        Task AddOrUpdateExchangeRateUSD(DateTime date, decimal exchangeRateUSD);

        Task AddOrUpdateExchangeRateEUR(DateTime date, decimal exchangeRateEUR);

        Task AddOrUpdateCreditRate(DateTime date, double creditRate);

        Task AddOrUpdateGDPIndicator(DateTime date, long gdpIndicator);

        Task AddOrUpdateImportIndicator(DateTime date, double importIndicator);

        Task AddOrUpdateExportIndicator(DateTime date, double exportIndicator);

        Task AddOrUpdateInflationIndex(DateTime date, double inflationIndex);

        #endregion
    }
}
