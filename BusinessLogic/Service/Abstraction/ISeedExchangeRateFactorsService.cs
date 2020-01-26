using System;
using System.Threading.Tasks;

namespace BusinessLogic.Service.Abstraction
{
    public interface ISeedExchangeRateFactorsService
    {
        Task FillExchangeRateUSD(DateTime date, decimal exchangeRateUSD);

        Task FillExchangeRateEUR(DateTime date, decimal exchangeRateEUR);

        Task FillCreditRate(DateTime date, double creditRate);

        Task FillGDPIndicator(DateTime date, long gdpIndicator);

        Task FillImportIndicator(DateTime date, double importIndicator);

        Task FillExportIndicator(DateTime date, double exportIndicator);

        Task FillInflationIndex(DateTime date, double inflationIndex);
    }
}
