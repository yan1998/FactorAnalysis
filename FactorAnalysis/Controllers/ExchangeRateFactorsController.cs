using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Models;
using BusinessLogic.Services.Abstractions;
using FactorAnalysis.Model.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FactorAnalysis.Controllers
{
    /// <summary>
    /// Controller for interact with ExchangeRateFactors
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateFactorsController : ControllerBase
    {
        private readonly ILogger<ExchangeRateFactorsController> _logger;
        private readonly IExchangeRateFactorsService _exchangeRateFactorsService;

        public ExchangeRateFactorsController(ILogger<ExchangeRateFactorsController> logger,
            IExchangeRateFactorsService exchangeRateFactorsService)
        {
            _logger = logger;
            _exchangeRateFactorsService = exchangeRateFactorsService;
        }

        /// <summary>
        /// Get ExchangeRateFactors for date range
        /// </summary> 
        [HttpGet("GetExchangeRateFactorsRange/{dateFrom}/{dateTo}")]
        public async Task<List<ExchangeRateFactors>> GetExchangeRateFactorsRange(DateTime dateFrom, DateTime dateTo)
        {
            return await _exchangeRateFactorsService.GetExchangeRateFactorsRange(dateFrom, dateTo);
        }

        /// <summary>
        /// Get Currency Exchange prediction result for USD
        /// </summary> 
        [HttpGet("GetUSDCurrencyExchangePrediction/{CreditRate}/{GDPIndicator}/{ImportIndicator}/{ExportIndicator}/{InflationIndex}")]
        public float GetUSDCurrencyExchangePrediction([FromRoute]CurrencyExchangePredictionRequest request)
        {
            ExchangeRateFactors factors = new ExchangeRateFactors
            {
                CreditRate = request.CreditRate,
                InflationIndex = request.InflationIndex,
                ImportIndicator = request.ImportIndicator,
                ExportIndicator = request.ExportIndicator,
                GDPIndicator = request.GDPIndicator
            };
            return _exchangeRateFactorsService.PredicateUSDCurrencyExchange(factors);
        }
    }
}