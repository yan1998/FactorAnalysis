using BusinessLogic.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BusinessLogic.Services.Abstractions
{
    public interface IExchangeRateFactorsService
    {
        Task<List<ExchangeRateFactors>> GetExchangeRateFactorsRange(DateTime dateFrom, DateTime dateTo);

        float PredicateUSDCurrencyExchange(ExchangeRateFactors factors);
    }
}
