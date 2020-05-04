using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Main controller for Managing Forecasting task entities
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ForecastingTaskEntityController : ControllerBase
    {
        private readonly ILogger<ForecastingTaskEntityController> _logger;
        private readonly IForecastingTasksService _forecastingTasksService;
        private readonly IMachineLearningService _machineLearningService;
        private readonly IImportExportInFileService _importExportInFileService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for controller instantiation
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="forecastingTasksService">Service for managing forecasting tasks</param>
        /// <param name="machineLearningService">Service for machine learning functionallity</param>
        /// <param name="importExportInFileService">Service for files functionallity</param>
        /// <param name="mapper">Mapper</param>
        public ForecastingTaskEntityController(ILogger<ForecastingTaskEntityController> logger,
            IForecastingTasksService forecastingTasksService,
            IMachineLearningService machineLearningService,
            IImportExportInFileService importExportInFileService,
            IMapper mapper)
        {
            _logger = logger;
            _forecastingTasksService = forecastingTasksService;
            _machineLearningService = machineLearningService;
            _importExportInFileService = importExportInFileService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all Forecasting Tasks name and description
        /// </summary>
        /// <returns>List of Forecasting Task entities information</returns>
        [HttpGet("ForecastingTaskEntities")]
        public async Task<List<GetForecastingTaskEntitiesResponse>> GetAllForecastingTaskEntities()
        {
            var shortInfo = await _forecastingTasksService.GetAllForecastingTaskEntities();
            return _mapper.Map<List<GetForecastingTaskEntitiesResponse>>(shortInfo);
        }

        /// <summary>
        /// Create new Forecasting Task
        /// </summary>
        /// <param name="request">Task creation request</param>
        [HttpPost("ForecastingTaskEntity")]
        public Task CreateForecastingTaskEntity([FromBody]CreateForecastingTaskEntityRequest request)
        {
            var declaration = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFieldDeclaration>>(request.FieldsDeclaration);
            return _forecastingTasksService.CreateForecastingTaskEntity(request.Name, request.Description, declaration);
        }

        /// <summary>
        /// Update name and description of the Forecasting Task entity
        /// </summary>
        /// <param name="request">Old and new names of the Forecasting Task entity with new description</param>
        [HttpPut("ForecastingTaskEntity")]
        public Task UpdateForecastingTaskEntity([FromBody]UpdateForecastingTaskEntityRequest request)
        {
            return _forecastingTasksService.EditForecastingTaskEntity(request.OldTaskName, request.NewTaskName, request.Description);
        }

        /// <summary>
        /// Delete the Forecasting Task entity
        /// </summary>
        /// <param name="taskEntityName">Name of the Forecasting Task entity</param>
        [HttpDelete("ForecastingTaskEntity/{taskEntityName}")]
        public Task DeleteForecastingTaskEntity(string taskEntityName)
        {
            return _forecastingTasksService.DeleteForecastingTaskEntity(taskEntityName);
        }

        /// <summary>
        /// Delete the record from the Forecasting Task entity
        /// </summary>
        /// <param name="taskEntityName">Name of the Forecasting Task Entity</param>
        /// <param name="recordId">Id of record</param>
        [HttpDelete("ForecastingTaskEntity/{taskEntityName}/{recordId}")]
        public Task DeleteForecastingTaskEntityRecord(string taskEntityName, string recordId)
        {
            return _forecastingTasksService.DeleteForecastingTaskRecordById(taskEntityName, recordId);
        }

        /// <summary>
        /// Add new record for the Forecasting Tasks entity
        /// </summary>
        /// <param name="taskEntityName">Name of the Forecasting Task Entity</param>
        /// <param name="request">Request for adding new record</param>
        [HttpPost("ForecastingTaskEntity/{taskEntityName}")]
        public Task AddForecstingTaskEntityRecord(string taskEntityName, [FromBody]AddForecstingTaskFactorsValueRequest request)
        {
            var values = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFieldValue>>(request.Values);
            return _forecastingTasksService.AddForecastingTaskRecord(taskEntityName, values);
        }

        /// <summary>
        /// Get declaration of the Forecasting Task entity
        /// </summary>
        /// <param name="taskEntityName">Name of the Forecasting Task entity</param>
        /// <returns>List of the Forecasting Task entity fields</returns>
        [HttpGet("TaskDeclaration/{taskEntityName}")]
        public async Task<GetForecastingTaskDeclarationResponse> GetForecastingTaskDeclaration(string taskEntityName)
        {
            var taskDeclaration = await _forecastingTasksService.GetForecastingTaskEntityDeclaration(taskEntityName);
            var response = new GetForecastingTaskDeclarationResponse { FieldsDeclaration = _mapper.Map<List<Model.ForecastingTaskFieldDeclaration>>(taskDeclaration)};
            return response;
        }

        /// <summary>
        /// Get filtered and paged the Forecasting Task records
        /// </summary>
        /// <param name="request">Search request</param>
        /// <returns>Paged and filtered information</returns>
        [HttpPost("PagedForecastingTaskEntity")]
        public async Task<GetPagedForecastingTaskResponse> GetPagedForecastingTaskRecords(GetPagedForecastingTaskRequest request)
        {
            var response = await _forecastingTasksService.SearchForecastingTaskRecords(_mapper.Map<SearchForecastingTaskRecords>(request));
            return _mapper.Map<GetPagedForecastingTaskResponse>(response);
        }

        /// <summary>
        /// Save a CSV file with the FOrecasting Task entity records
        /// </summary>
        /// <param name="taskEntityName">Name of the Forecasting Task entity</param>
        /// <returns>Content result with file</returns>
        [HttpGet("SaveForecastingTaskValuesCsv/{taskEntityName}")]
        public async Task<ContentResult> SaveForecastingTaskValuesCsvFile(string taskEntityName)
        {
            var response = await _importExportInFileService.GenerateCsvString(taskEntityName);

            ContentResult result = new ContentResult();
            result.Content = response;
            result.ContentType = "text/csv";

            return result;
        }

        /// <summary>
        /// Upload a CSV file with data
        /// </summary>
        /// <param name="taskEntityName">Name of the Forecasting Task entity</param>
        [HttpPost("UploadCsvFile/{taskEntityName}"), DisableRequestSizeLimit]
        public Task UploadCsvFile(string taskEntityName)
        {
            string csv = StreamConversionHelper.ConvertStreamToString(Request.Form.Files[0].OpenReadStream());
            return _importExportInFileService.AddForecastingTaskRecordsViaCsv(taskEntityName, csv);
        }

        /// <summary>
        /// Save a JSON file with the Forecasting Task entity records
        /// </summary>
        /// <param name="taskEntityName">Name of the Forecasting Task entity</param>
        /// <returns>Content result with file</returns>
        [HttpGet("SaveForecastingTaskValuesJson/{taskEntityName}")]
        public async Task<ContentResult> SaveForecastingTaskValuesJsonFile(string taskEntityName)
        {
            var response = await _importExportInFileService.GenerateJsonString(taskEntityName);

            ContentResult result = new ContentResult();
            result.Content = response;
            result.ContentType = "application/json";

            return result;
        }

        /// <summary>
        /// Save a XML file with the Forecasting Task entity records
        /// </summary>
        /// <param name="taskEntityName">Name of the Forecasting Task entity</param>
        /// <returns>Content result with file</returns>
        [HttpGet("SaveForecastingTaskValuesXml/{taskEntityName}")]
        public async Task<ContentResult> SaveForecastingTaskValuesXmlFile(string taskEntityName)
        {
            var response = await _importExportInFileService.GenerateXmlString(taskEntityName);

            ContentResult result = new ContentResult();
            result.Content = response;
            result.ContentType = "application/xml";

            return result;
        }

        /// <summary>
        /// Create a prediction model for the Forecasting Task entity
        /// </summary>
        /// <param name="taskEntityName">Name of the Forecasting Task entity</param>
        /// <param name="learningAlgorithm">Name of learning algorithm</param>
        [HttpPost("CreateTaskEntityPredictionModel/{taskEntityName}/{learningAlgorithm}")]
        public Task CreateTaskEntityPredictionModel(string taskEntityName, LearningAlgorithm learningAlgorithm)
        {
            return _machineLearningService.CreateForecastingTaskMLModel(taskEntityName, learningAlgorithm);
        }

        /// <summary>
        /// Predict value for the Forecasting Task entity
        /// </summary>
        /// <param name="taskEntityName">Name of the Forecasting Task entity</param>
        /// <param name="request">List of factor values</param>
        /// <returns>Predicted value</returns>
        [HttpPost("PredictValue/{taskEntityName}")]
        public Task<float> PredictValue(string taskEntityName, [FromBody]PredictValueRequest request)
        {
            var factorValuesDomain =_mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFieldValue>>(request.Values);
            return _machineLearningService.PredictValueByFactors(taskEntityName, factorValuesDomain);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request">Request with Forecasting Task entity name and list of names of learning algorithms</param>
        /// <returns>Reports of training and prediction</returns>
        [HttpPost("AnalyzePredictionAlgorithms")]
        public async Task<AnalyzePredictionAlgorithmsResponse> AnalyzePredictionAlgorithms(AnalyzePredictionAlgorithmsRequest request)
        {
            var algorithms = new List<LearningAlgorithm>();
            foreach (var algorithm in request.Algorithms)
            {
                algorithms.Add((LearningAlgorithm)Enum.Parse(typeof(LearningAlgorithm), algorithm));
            }

            var result = await _machineLearningService.AnalyzePredictionAlgorithms(request.TaskEntityName, algorithms);
            var response = new AnalyzePredictionAlgorithmsResponse
            {
                Reports = _mapper.Map<List<Model.AnalyzePredictionAlgorithmsReport>>(result)
            };
            return response;
        }
    }
}