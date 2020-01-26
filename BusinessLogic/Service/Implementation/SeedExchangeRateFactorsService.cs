using BusinessLogic.Service.Abstraction;
using DataAccess.Repository.Abstraction;
using System;
using System.Threading.Tasks;

namespace BusinessLogic.Service.Implementation
{
    public class SeedExchangeRateFactorsService : ISeedExchangeRateFactorsService
    {
        private readonly IExchangeRateFactorsRepository _exchangeRateFactorsRepository;

        public SeedExchangeRateFactorsService(IExchangeRateFactorsRepository exchangeRateFactorsRepository)
        {
            _exchangeRateFactorsRepository = exchangeRateFactorsRepository;
        }

        public Task FillCreditRate(DateTime date, double creditRate)
        {
            return _exchangeRateFactorsRepository.AddOrUpdateCreditRate(date, creditRate);
        }

        public Task FillExchangeRateEUR(DateTime date, decimal exchangeRateEUR)
        {
            return _exchangeRateFactorsRepository.AddOrUpdateExchangeRateEUR(date, exchangeRateEUR);
        }

        public Task FillExchangeRateUSD(DateTime date, decimal exchangeRateUSD)
        {
            return _exchangeRateFactorsRepository.AddOrUpdateExchangeRateUSD(date, exchangeRateUSD);
        }

        public Task FillExportIndicator(DateTime date, double exportIndicator)
        {
            return _exchangeRateFactorsRepository.AddOrUpdateExportIndicator(date, exportIndicator);
        }

        public Task FillGDPIndicator(DateTime date, long gdpIndicator)
        {
            return _exchangeRateFactorsRepository.AddOrUpdateGDPIndicator(date, gdpIndicator);
        }

        public Task FillImportIndicator(DateTime date, double importIndicator)
        {
            return _exchangeRateFactorsRepository.AddOrUpdateImportIndicator(date, importIndicator);
        }

        public Task FillInflationIndex(DateTime date, double inflationIndex)
        {
            return _exchangeRateFactorsRepository.AddOrUpdateInflationIndex(date, inflationIndex);
        }
    }
}
