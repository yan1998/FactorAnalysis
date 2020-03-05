using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Services.Abstractions;
using DomainModel.ExchangeRateFactors;
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
        [HttpGet("ExchangeRateFactorsRange/{dateFrom}/{dateTo}")]
        public Task<List<ExchangeRateFactors>> GetExchangeRateFactorsRange(DateTime dateFrom, DateTime dateTo)
        {
            return _exchangeRateFactorsService.GetExchangeRateFactorsRange(dateFrom, dateTo);
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

        /// <summary>
        /// Get Currency Exchange rate factors record by id
        /// </summary>
        /// <param name="id">Database id field</param>
        /// <returns></returns>
        [HttpGet("ExchangeRateFactors/{id}")]
        public Task<ExchangeRateFactors> ExchangeRateFactorsById(int id)
        {
            return _exchangeRateFactorsService.GetExchangeRateFactorsById(id);
        }

        /// <summary>
        /// Get Currency Exchange rate factors with pagination support
        /// </summary>
        /// <param name="pageNumber">Number of page</param>
        /// <param name="perPage">Count of record per page</param>
        /// <returns></returns>
        [HttpGet("PagedExchangeRateFactors/{pageNumber}/{perPage}")]
        public Task<PagedExchangeRateFactors> PagedExchangeRateFactors(int pageNumber, int perPage)
        {
            return _exchangeRateFactorsService.GetPagedExchangeRateFactors(pageNumber, perPage);
        }

        /// <summary>
        /// Create new ExchangeRateFactors record to database
        /// </summary>
        /// <param name="request">Creation request</param>
        /// <returns></returns>
        [HttpPost("ExchangeRateFactors")]
        public Task CreateExchangeRateFactors([FromBody]ExchangeRateFactors request)
        {
            return _exchangeRateFactorsService.CreateExchangeRateFactors(request);
        }

        /// <summary>
        /// Update ExchangeRateFactors record by id
        /// </summary>
        /// <param name="id">Id of record</param>
        /// <param name="request">Creation request</param>
        /// <returns></returns>
        [HttpPut("ExchangeRateFactors/{id}")]
        public Task UpdateExchangeRateFactors(int id, [FromBody]ExchangeRateFactors request)
        {
            request.Id = id;
            return _exchangeRateFactorsService.UpdateExchangeRateFactors(request);
        }

        /// <summary>
        /// Remove ExchangeRateFactors record from Database by id
        /// </summary>
        /// <param name="id">>Id of record</param>
        /// <returns></returns>
        [HttpDelete("ExchangeRateFactors/{id}")]
        public Task DeleteExchangeRateFactors(int id)
        {
            return _exchangeRateFactorsService.RemoveExchangeRateFactors(id);
        }
    }
}