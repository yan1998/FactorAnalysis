using System;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Abstractions
{
    public interface ISeedExchangeRateFactorsService
    {
        Task FillExchangeRateUSD(string filePath);

        Task FillExchangeRateEUR(string filePath);

        Task FillCreditRate(string filePath);

        Task FillGDPIndicator(string filePath);

        Task FillImportIndicator(string filePath);

        Task FillExportIndicator(string filePath);

        Task FillInflationIndex(string filePath);
    }
}
