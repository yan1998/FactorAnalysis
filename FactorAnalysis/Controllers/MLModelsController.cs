using BusinessLogic.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FactorAnalysis.Controllers
{
    /// <summary>
    /// Controller for interact with ML.NET
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class MLModelsController : ControllerBase
    {
        private readonly ILogger<SeedExchangeRateFactorsController> _logger;
        private readonly IExchangeRateFactorsService _exchangeRateFactorsService;


        public MLModelsController(ILogger<SeedExchangeRateFactorsController> logger,
            IExchangeRateFactorsService exchangeRateFactorsService)
        {
            _logger = logger;
            _exchangeRateFactorsService = exchangeRateFactorsService;
        }

        /// <summary>
        /// Create Predication model for EURCurrencyExchange
        /// </summary>
        [HttpPost("CreateEURCurrencyExchangeModel")]
        public Task CreateEURCurrencyExchangeModel()
        {
            return _exchangeRateFactorsService.CreateEURCurrencyExchangeMLModel();
        }

        /// <summary>
        /// Retrain Predication model for EURCurrencyExchange
        /// </summary>
        [HttpPut("RetrainEURCurrencyExchangeModel")]
        public Task RetrainEURCurrencyExchangeModel()
        {
            return _exchangeRateFactorsService.RetrainEURCurrencyExchangeMLModel();
        }
    }
}