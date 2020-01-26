using DataAccess.Model;
using DataAccess.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DataAccess.Repository.Implementation
{
    public class ExchangeRateFactorsRepository : IExchangeRateFactorsRepository
    {
        public readonly DatabaseContext _context;

        public ExchangeRateFactorsRepository(DatabaseContext context)
        {
            _context = context;
        }

        public Task<ExchangeRateFactors> GetExchangeRateFactorsByDate(DateTime date)
        {
            return _context.ExchangeRateFactors.AsNoTracking().FirstOrDefaultAsync(erf => erf.Date.Date == date.Date);
        }

        #region SeedData

        public async Task AddOrUpdateCreditRate(DateTime date, double creditRate)
        {
            var exchangeRateFactors = await GetExchangeRateFactorsByDateInternal(date);

            if (exchangeRateFactors == null)
            {
                exchangeRateFactors = new ExchangeRateFactors
                {
                    Date = date.Date
                };
            }
            exchangeRateFactors.CreditRate = creditRate;
            await _context.SaveChangesAsync();
        }

        public async Task AddOrUpdateExchangeRateEUR(DateTime date, decimal exchangeRateEUR)
        {
            var exchangeRateFactors = await GetExchangeRateFactorsByDateInternal(date);

            if (exchangeRateFactors == null)
            {
                exchangeRateFactors = new ExchangeRateFactors
                {
                    Date = date.Date
                };
            }
            exchangeRateFactors.ExchangeRateEUR = exchangeRateEUR;
            await _context.SaveChangesAsync();
        }

        public async Task AddOrUpdateExchangeRateUSD(DateTime date, decimal exchangeRateUSD)
        {
            var exchangeRateFactors = await GetExchangeRateFactorsByDateInternal(date);

            if (exchangeRateFactors == null)
            {
                exchangeRateFactors = new ExchangeRateFactors
                {
                    Date = date.Date
                };
            }
            exchangeRateFactors.ExchangeRateUSD = exchangeRateUSD;
            await _context.SaveChangesAsync();
        }

        public async Task AddOrUpdateExportIndicator(DateTime date, double exportIndicator)
        {
            var exchangeRateFactors = await GetExchangeRateFactorsByDateInternal(date);

            if (exchangeRateFactors == null)
            {
                exchangeRateFactors = new ExchangeRateFactors
                {
                    Date = date.Date
                };
            }
            exchangeRateFactors.ExportIndicator = exportIndicator;
            await _context.SaveChangesAsync();
        }

        public async Task AddOrUpdateGDPIndicator(DateTime date, long gdpIndicator)
        {
            var exchangeRateFactors = await GetExchangeRateFactorsByDateInternal(date);

            if (exchangeRateFactors == null)
            {
                exchangeRateFactors = new ExchangeRateFactors
                {
                    Date = date.Date
                };
            }
            exchangeRateFactors.GDPIndicator = gdpIndicator;
            await _context.SaveChangesAsync();
        }

        public async Task AddOrUpdateImportIndicator(DateTime date, double importIndicator)
        {
            var exchangeRateFactors = await GetExchangeRateFactorsByDateInternal(date);

            if (exchangeRateFactors == null)
            {
                exchangeRateFactors = new ExchangeRateFactors
                {
                    Date = date.Date
                };
            }
            exchangeRateFactors.ImportIndicator = importIndicator;
            await _context.SaveChangesAsync();
        }

        public async Task AddOrUpdateInflationIndex(DateTime date, double inflationIndex)
        {
            var exchangeRateFactors = await GetExchangeRateFactorsByDateInternal(date);

            if (exchangeRateFactors == null)
            {
                exchangeRateFactors = new ExchangeRateFactors
                {
                    Date = date.Date
                };
            }
            exchangeRateFactors.InflationIndex = inflationIndex;
            await _context.SaveChangesAsync();
        }

        #endregion

        private Task<ExchangeRateFactors> GetExchangeRateFactorsByDateInternal(DateTime date)
        {
            return _context.ExchangeRateFactors.FirstOrDefaultAsync(erf => erf.Date.Date == date.Date);
        }
    }
}
