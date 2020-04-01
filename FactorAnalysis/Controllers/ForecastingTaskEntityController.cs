﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Services.Abstractions;
using CsvHelper;
using FactorAnalysis.Helpers;
using FactorAnalysis.Model.Requests;
using FactorAnalysis.Model.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FactorAnalysis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForecastingTaskEntityController : ControllerBase
    {
        private readonly ILogger<ForecastingTaskEntityController> _logger;
        private readonly IForecastingTasksService _forecastingTasksService;
        private readonly IMapper _mapper;

        public ForecastingTaskEntityController(ILogger<ForecastingTaskEntityController> logger,
            IForecastingTasksService forecastingTasksService,
            IMapper mapper)
        {
            _logger = logger;
            _forecastingTasksService = forecastingTasksService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all forecasting tasks name from database
        /// </summary>
        /// <returns>List of entities name</returns>
        [HttpGet("ForecastingTaskEntitiesNames")]
        public Task<List<string>> GetForecastingTaskEntities()
        {
            return _forecastingTasksService.GetAllForecastingTaskEntitiesName();
        }

        /// <summary>
        /// Create new forecasting task in database
        /// </summary>
        /// <param name="request">Creation request</param>
        /// <returns></returns>
        [HttpPost("ForecastingTaskEntity")]
        public Task CreateForecastingTaskEntity([FromBody]CreateForecastingTaskEntityRequest request)
        {
            var declaration = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFactorDeclaration>>(request.TaskFactorsDeclaration);
            return _forecastingTasksService.CreateForecastingTaskEntity(request.TaskEntityName, declaration);
        }

        /// <summary>
        /// Delete forecasting task from database
        /// </summary>
        /// <param name="taskEntityName">Name of forecasting task</param>
        /// <returns></returns>
        [HttpDelete("ForecastingTaskEntity/{taskEntityName}")]
        public Task DeleteForecastingTaskEntity(string taskEntityName)
        {
            return _forecastingTasksService.DeleteForecastingTaskEntity(taskEntityName);
        }

        /// <summary>
        /// Delete an array of factors values for task entity
        /// </summary>
        /// <param name="taskEntityName">Name of forecasting task</param>
        /// <param name="id">Id of values array</param>
        /// <returns></returns>
        [HttpDelete("ForecastingTaskEntity/{taskEntityName}/{id}")]
        public Task DeleteForecastingTaskEntityValues(string taskEntityName, string id)
        {
            return _forecastingTasksService.DeleteForecastingTaskFactorsById(taskEntityName, id);
        }

        /// <summary>
        /// Add new factors values to database
        /// </summary>
        /// <param name="taskEntityName">Name of forecasting task</param>
        /// <param name="request">Request for adding new factors value</param>
        /// <returns></returns>
        [HttpPost("ForecastingTaskEntity/{taskEntityName}")]
        public Task AddForecstingTaskFactorsValue(string taskEntityName, [FromBody]AddForecstingTaskFactorsValueRequest request)
        {
            var values = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFactorValue>>(request.Values);
            return _forecastingTasksService.AddForecastingTaskFactors(taskEntityName, values);
        }

        /// <summary>
        /// Get paged ForecastingTask factors value
        /// </summary>
        /// <param name="taskEntityName">Name of forecasting task</param>
        /// <param name="pageNumber">Number of page</param>
        /// <param name="perPage">Amount of record per page</param>
        /// <returns>Paged information</returns>
        [HttpGet("PagedForecastingTaskEntity/{taskEntityName}/{pageNumber}/{perPage}")]
        public async Task<GetPagedForecastingTaskResponse> GetPagedForecastingTask(string taskEntityName, int pageNumber, int perPage)
        {
            var response = await _forecastingTasksService.GetPagedForecastingTaskEntity(taskEntityName, pageNumber, perPage);
            return _mapper.Map<GetPagedForecastingTaskResponse>(response);
        }

        /// <summary>
        /// Save csv file with factor values
        /// </summary>
        /// <param name="taskEntityName">Name of forecasting task</param>
        /// <returns></returns>
        [HttpGet("SaveForecastingTaskValuesCsv/{taskEntityName}")]
        public async Task<ContentResult> SaveForecastingTaskValuesCsv(string taskEntityName)
        {
            var response = await _forecastingTasksService.GetForecastingTaskEntityForCsv(taskEntityName);

            ContentResult result = new ContentResult();
            result.Content = response;
            result.ContentType = "text/csv";

            return result;
        }


        [HttpPost("UploadCsvFile/{taskEntityName}"), DisableRequestSizeLimit]
        public Task UploadCsvFile(string taskEntityName)
        {
            string csv = StreamConversionHelper.ConvertStreamToString(Request.Form.Files[0].OpenReadStream());
            return _forecastingTasksService.AddForecastingTaskFactorsViaCsv(taskEntityName, csv);
        }
    }
}