using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Services.Abstractions;
using DomainModel.ForecastingTasks;
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
        [HttpGet("ForecastingTaskEntities")]
        public async Task<List<GetForecastingTaskEntitiesResponse>> GetForecastingTaskEntities()
        {
            var shortInfo = await _forecastingTasksService.GetAllForecastingTaskEntities();
            return _mapper.Map<List<GetForecastingTaskEntitiesResponse>>(shortInfo);
        }

        /// <summary>
        /// Create new forecasting task in database
        /// </summary>
        /// <param name="request">Creation request</param>
        /// <returns></returns>
        [HttpPost("ForecastingTaskEntity")]
        public Task CreateForecastingTaskEntity([FromBody]CreateForecastingTaskEntityRequest request)
        {
            var declaration = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFieldDeclaration>>(request.FieldsDeclaration);
            return _forecastingTasksService.CreateForecastingTaskEntity(request.Name, request.Description, declaration);
        }

        /// <summary>
        /// Update name and description for Task Entity
        /// </summary>
        /// <param name="request">old and new names for task entity. And new description</param>
        /// <returns></returns>
        [HttpPut("ForecastingTaskEntity")]
        public Task UpdateForecastingTaskEntity([FromBody]UpdateForecastingTaskEntityRequest request)
        {
            return _forecastingTasksService.RenameForecastingTaskEntity(request.OldTaskName, request.NewTaskName, request.Description);
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
            var values = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFieldValue>>(request.Values);
            return _forecastingTasksService.AddForecastingTaskFactors(taskEntityName, values);
        }

        /// <summary>
        /// Gets forecasting task entity field declaration
        /// </summary>
        /// <param name="taskEntityName">Name of forecasting task</param>
        /// <returns>List of task fields</returns>
        [HttpGet("TaskDeclaration/{taskEntityName}")]
        public async Task<GetForecastingTaskDeclarationResponse> GetForecastingTaskDeclaration(string taskEntityName)
        {
            var taskDeclaration = await _forecastingTasksService.GetForecastingTaskEntityDeclaration(taskEntityName);
            var response = new GetForecastingTaskDeclarationResponse { FieldsDeclaration = _mapper.Map<List<Model.ForecastingTaskFieldDeclaration>>(taskDeclaration)};
            return response;
        }

        /// <summary>
        /// Get paged ForecastingTask factors value
        /// </summary>
        /// <param name="request">Search request</param>
        /// <returns>Paged information</returns>
        [HttpPost("PagedForecastingTaskEntity")]
        public async Task<GetPagedForecastingTaskResponse> GetPagedForecastingTask(GetPagedForecastingTaskRequest request)
        {
            var response = await _forecastingTasksService.SearchForecastingTaskRecords(_mapper.Map<SearchForecastingTaskRecords>(request));
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

        /// <summary>
        /// Uploading a csv file with data
        /// </summary>
        /// <param name="taskEntityName">Name of forecasting task</param>
        /// <returns></returns>
        [HttpPost("UploadCsvFile/{taskEntityName}"), DisableRequestSizeLimit]
        public Task UploadCsvFile(string taskEntityName)
        {
            string csv = StreamConversionHelper.ConvertStreamToString(Request.Form.Files[0].OpenReadStream());
            return _forecastingTasksService.AddForecastingTaskFactorsViaCsv(taskEntityName, csv);
        }

        /// <summary>
        /// Create prediction model for concrete taskEntity
        /// </summary>
        /// <param name="taskEntityName">Name of forecasting task</param>
        /// <param name="learningAlgorithm">Name of learning algorithm</param>
        /// <returns></returns>
        [HttpPost("CreateTaskEntityPredictionModel/{taskEntityName}/{learningAlgorithm}")]
        public Task CreateTaskEntityPredictionModel(string taskEntityName, LearningAlgorithm learningAlgorithm)
        {
            return _forecastingTasksService.CreateForecastingTaskMLModel(taskEntityName, learningAlgorithm);
        }

        /// <summary>
        /// Prediction method
        /// </summary>
        /// <param name="taskEntityName">Name of forecasting task</param>
        /// <param name="request">List of factor values</param>
        /// <returns>Prediction value</returns>
        [HttpPost("PredictValue/{taskEntityName}")]
        public Task<float> PredictValue(string taskEntityName, [FromBody]PredictValueRequest request)
        {
            var factorValuesDomain =_mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFieldValue>>(request.Values);
            return _forecastingTasksService.PredictValue(taskEntityName, factorValuesDomain);
        }
    }
}