using DomainModel.ExchangeRateFactors;
using DataAccess.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace DataAccess.Repositories.Implementations
{
    public class ExchangeRateFactorsRepository : IExchangeRateFactorsRepository
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public ExchangeRateFactorsRepository(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ExchangeRateFactors> GetExchangeRateFactorsByDate(DateTime date)
        {
            var sqlFactors = await _context.ExchangeRateFactors.AsNoTracking().FirstOrDefaultAsync(erf => erf.Date.Date == date.Date);
            return _mapper.Map<ExchangeRateFactors>(sqlFactors);
        }

        public async Task<List<ExchangeRateFactors>> GetExchangeRateFactorsRange(DateTime dateFrom, DateTime dateTo)
        {
            var sqlFactors = await _context.ExchangeRateFactors.AsNoTracking().Where(erf => erf.Date >= dateFrom && erf.Date <= dateTo).ToListAsync();
            return _mapper.Map<List<ExchangeRateFactors>>(sqlFactors);
        }

        public async Task<ExchangeRateFactors> GetExchangeRateFactorsById(int id)
        {
            var sqlFactors = await _context.ExchangeRateFactors.AsNoTracking().FirstAsync(erf => erf.Id == id);
            return _mapper.Map<ExchangeRateFactors>(sqlFactors);
        }

        public async Task<PagedExchangeRateFactors> GetPagedExchangeRateFactors(int pageNumber, int perPage)
        {
            var sqlFactors = await _context.ExchangeRateFactors.AsNoTracking().Skip(pageNumber * perPage).Take(perPage).ToListAsync();
            var result = new PagedExchangeRateFactors
            {
                ExchangeRateFactors = _mapper.Map<List<ExchangeRateFactors>>(sqlFactors),
                TotalAmount = await _context.ExchangeRateFactors.CountAsync()
            };
            return result;
        }

        public async Task CreateExchangeRateFactors(ExchangeRateFactors factors)
        {
            var sqlFactors = _mapper.Map<Model.ExchangeRateFactors>(factors);
            _context.ExchangeRateFactors.Add(sqlFactors);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateExchangeRateFactors(ExchangeRateFactors factors)
        {
            var sqlFactors = _mapper.Map<Model.ExchangeRateFactors>(factors);
            var attachedFactors = _context.ExchangeRateFactors.Attach(sqlFactors);
            attachedFactors.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveExchangeRateFactors(int id)
        {
            var factors = await _context.ExchangeRateFactors.FirstAsync(erf => erf.Id == id);
            _context.ExchangeRateFactors.Remove(factors);
            await _context.SaveChangesAsync();
        }

        #region SeedData

        public async Task AddOrUpdateCreditRate(DateTime date, float creditRate)
        {
            var exchangeRateFactors = await GetExchangeRateFactorsByDateInternal(date);

            if (exchangeRateFactors == null)
            {
                exchangeRateFactors = new Model.ExchangeRateFactors
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
                exchangeRateFactors = new Model.ExchangeRateFactors
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
                exchangeRateFactors = new Model.ExchangeRateFactors
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
                exchangeRateFactors = new Model.ExchangeRateFactors
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
                exchangeRateFactors = new Model.ExchangeRateFactors
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
                exchangeRateFactors = new Model.ExchangeRateFactors
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
                exchangeRateFactors = new Model.ExchangeRateFactors
                {
                    Date = date.Date
                };
                _context.ExchangeRateFactors.Add(exchangeRateFactors);
            }
            exchangeRateFactors.InflationIndex = inflationIndex;
            await _context.SaveChangesAsync();
        }

        #endregion

        private Task<Model.ExchangeRateFactors> GetExchangeRateFactorsByDateInternal(DateTime date)
        {
            return _context.ExchangeRateFactors.FirstOrDefaultAsync(erf => erf.Date.Date == date.Date);
        }
    }
}
