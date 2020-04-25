using DomainModel.ForecastingTasks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Abstractions
{
    public interface IForecastingTasksService
    {
        Task<List<ShortForecastingTaskInfo>> GetAllForecastingTaskEntities();

        Task CreateForecastingTaskEntity(string entityName, string description, List<ForecastingTaskFieldDeclaration> declaration);

        Task RenameForecastingTaskEntity(string oldTaskName, string newTaskName, string newTaskDescription);

        Task DeleteForecastingTaskEntity(string entityName);

        Task AddForecastingTaskFactors(string entityName, List<ForecastingTaskFieldValue> values);

        Task DeleteForecastingTaskFactorsById(string entityName, string id);

        Task<PagedForecastingTask> SearchForecastingTaskRecords(SearchForecastingTaskRecords searchRequest);

        Task<string> GetForecastingTaskEntityForCsv(string entityName);

        Task AddForecastingTaskFactorsViaCsv(string entityName, string csv);

        Task CreateForecastingTaskMLModel(string entityName, LearningAlgorithm learningAlgorith);

        Task<float> PredictValue(string entityName, List<ForecastingTaskFieldValue> values);
    }
}
