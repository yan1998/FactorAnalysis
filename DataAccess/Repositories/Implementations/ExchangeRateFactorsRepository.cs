using DataAccess.Model;
using DataAccess.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Implementations
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

        public Task<List<ExchangeRateFactors>> GetExchangeRateFactorsRange(DateTime dateFrom, DateTime dateTo)
        {
            return _context.ExchangeRateFactors.AsNoTracking().Where(erf => erf.Date >= dateFrom && erf.Date <= dateTo).ToListAsync();
        }

        #region SeedData

        public async Task AddOrUpdateCreditRate(DateTime date, float creditRate)
        {
            var exchangeRateFactors = await GetExchangeRateFactorsByDateInternal(date);

            if (exchangeRateFactors == null)
            {
                exchangeRateFactors = new ExchangeRateFactors
                {
                    Date = date.Date
                };
                _context.ExchangeRateFactors.Add(exchangeRateFactors);
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
                _context.ExchangeRateFactors.Add(exchangeRateFactors);
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
                _context.ExchangeRateFactors.Add(exchangeRateFactors);
            }
            exchangeRateFactors.ExchangeRateUSD = exchangeRateUSD;
            await _context.SaveChangesAsync();
        }

        public async Task AddOrUpdateExportIndicator(DateTime date, float exportIndicator)
        {
            var exchangeRateFactors = await GetExchangeRateFactorsByDateInternal(date);

            if (exchangeRateFactors == null)
            {
                exchangeRateFactors = new ExchangeRateFactors
                {
                    Date = date.Date
                };
                _context.ExchangeRateFactors.Add(exchangeRateFactors);
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
                _context.ExchangeRateFactors.Add(exchangeRateFactors);
            }
            exchangeRateFactors.GDPIndicator = gdpIndicator;
            await _context.SaveChangesAsync();
        }

        public async Task AddOrUpdateImportIndicator(DateTime date, float importIndicator)
        {
            var exchangeRateFactors = await GetExchangeRateFactorsByDateInternal(date);

            if (exchangeRateFactors == null)
            {
                exchangeRateFactors = new ExchangeRateFactors
                {
                    Date = date.Date
                };
                _context.ExchangeRateFactors.Add(exchangeRateFactors);
            }
            exchangeRateFactors.ImportIndicator = importIndicator;
            await _context.SaveChangesAsync();
        }

        public async Task AddOrUpdateInflationIndex(DateTime date, float inflationIndex)
        {
            var exchangeRateFactors = await GetExchangeRateFactorsByDateInternal(date);

            if (exchangeRateFactors == null)
            {
                exchangeRateFactors = new ExchangeRateFactors
                {
                    Date = date.Date
                };
                _context.ExchangeRateFactors.Add(exchangeRateFactors);
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
