using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using BusinessLogic.Services.Abstractions;
using FactorAnalysis.Model.Requests;

namespace FactorAnalysis.Controllers
{
    /// <summary>
    /// Controller for Seed data to ExchangeRateFactors table
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class SeedExchangeRateFactorsController : ControllerBase
    {
        private readonly ILogger<SeedExchangeRateFactorsController> _logger;
        private readonly ISeedExchangeRateFactorsService _seedExchangeRateFactorsService;

        public SeedExchangeRateFactorsController(ILogger<SeedExchangeRateFactorsController> logger,
            ISeedExchangeRateFactorsService seedExchangeRateFactorsService)
        {
            _logger = logger;
            _seedExchangeRateFactorsService = seedExchangeRateFactorsService;
        }

        /// <summary>
        /// Seed ExchangeRateEUR field to Database
        /// SeedData/ExchangeRateEUR.csv
        /// </summary> 
        [HttpPost("SeedExchangeRateEUR")]
        public async Task SeedExchangeRateEUR([FromBody]SeedFilePathRequest filePathRequest)
        {
            await _seedExchangeRateFactorsService.FillExchangeRateEUR(filePathRequest.FilePath);
        }

        /// <summary>
        /// Seed ExchangeRateUSD field to Database
        /// SeedData/ExchangeRateUSD.csv
        /// </summary> 
        [HttpPost("SeedExchangeRateUSD")]
        public async Task SeedExchangeRateUSD([FromBody]SeedFilePathRequest filePathRequest)
        {
            await _seedExchangeRateFactorsService.FillExchangeRateUSD(filePathRequest.FilePath);
        }

        /// <summary>
        /// Seed CreditRate field to Database
        /// SeedData/CreditRate.csv
        /// </summary> 
        [HttpPost("SeedCreditRate")]
        public async Task SeedCreditRate([FromBody]SeedFilePathRequest filePathRequest)
        {
            await _seedExchangeRateFactorsService.FillCreditRate(filePathRequest.FilePath);
        }

        /// <summary>
        /// Seed ExportIndicator field to Database
        /// SeedData/ExportIndicator.csv
        /// </summary> 
        [HttpPost("SeedExportIndicator")]
        public async Task SeedExportIndicator([FromBody]SeedFilePathRequest filePathRequest)
        {
            await _seedExchangeRateFactorsService.FillExportIndicator(filePathRequest.FilePath);
        }

        /// <summary>
        /// Seed ImportIndicator field to Database
        /// SeedData/ImportIndicator.csv
        /// </summary> 
        [HttpPost("SeedImportIndicator")]
        public async Task SeedImportIndicator([FromBody]SeedFilePathRequest filePathRequest)
        {
            await _seedExchangeRateFactorsService.FillImportIndicator(filePathRequest.FilePath);
        }
    }
}
