using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ExchangeRateFactorsController(ILogger<ExchangeRateFactorsController> logger,
            IExchangeRateFactorsService exchangeRateFactorsService,
            IMapper mapper)
        {
            _logger = logger;
            _exchangeRateFactorsService = exchangeRateFactorsService;
            _mapper = mapper;
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
        [HttpGet("PredictUSDCurrencyExchange/{CreditRate}/{GDPIndicator}/{ImportIndicator}/{ExportIndicator}/{InflationIndex}")]
        public float PredictUSDCurrencyExchange([FromRoute]CurrencyExchangePredictionRequest request)
        {
            var factors = _mapper.Map<ExchangeRateFactors>(request);
            return _exchangeRateFactorsService.PredictUSDCurrencyExchange(factors);
        }

        /// <summary>
        /// Get Currency Exchange prediction result for EUR
        /// </summary> 
        [HttpGet("PredictEURCurrencyExchange/{CreditRate}/{GDPIndicator}/{ImportIndicator}/{ExportIndicator}/{InflationIndex}")]
        public float PredictEURCurrencyExchange([FromRoute]CurrencyExchangePredictionRequest request)
        {
            var factors = _mapper.Map<ExchangeRateFactors>(request);
            return _exchangeRateFactorsService.PredictEURCurrencyExchange(factors);
        }
    }
}