using DomainModel.ForecastingTasks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Abstractions
{
    public interface IForecastingTasksRepository
    {
        Task<List<ShortForecastingTaskInfo>> GetAllForecastingTaskEntities();

        Task CreateForecastingTaskEntity(string taskName, string description, List<ForecastingTaskFieldDeclaration> declaration);

        Task EditForecastingTaskEntity(string oldTaskName, string newTaskName, string newTaskDescription);

        Task DeleteForecastingTaskEntity(string taskName);

        Task<ForecastingTask> GetForecastingTaskEntity(string taskName);

        Task<PagedForecastingTask> SearchForecastingTaskRecords(SearchForecastingTaskRecords searchRequest);

        Task AddForecastingTaskFields(string taskName, List<ForecastingTaskFieldValue> values);

        Task AddBatchOfForecastingTaskFields(string taskName, List<List<ForecastingTaskFieldValue>> values);

        Task DeleteForecastingTaskFieldsById(string taskName, string id);

        Task<List<ForecastingTaskFieldDeclaration>> GetForecastingTaskFieldsDeclaration(string taskName);
    }
}
