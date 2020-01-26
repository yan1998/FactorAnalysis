using DataAccess.Repository.Abstraction;
using System;
using System.Threading.Tasks;

namespace DataAccess.Repository.Implementation
{
    public class ExchangeRateFactorsRepository : IExchangeRateFactorsRepository
    {

        #region SeedData

        public Task AddOrUpdateCreditRate(DateTime date, double creditRate)
        {
            throw new NotImplementedException();
        }

        public Task AddOrUpdateExchangeRateEUR(DateTime date, decimal exchangeRateEUR)
        {
            throw new NotImplementedException();
        }

        public Task AddOrUpdateExchangeRateUSD(DateTime date, decimal exchangeRateUSD)
        {
            throw new NotImplementedException();
        }

        public Task AddOrUpdateExportIndicator(DateTime date, double exportIndicator)
        {
            throw new NotImplementedException();
        }

        public Task AddOrUpdateGDPIndicator(DateTime date, long gdpIndicator)
        {
            throw new NotImplementedException();
        }

        public Task AddOrUpdateImportIndicator(DateTime date, double importIndicator)
        {
            throw new NotImplementedException();
        }

        public Task AddOrUpdateInflationIndex(DateTime date, double inflationIndex)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
