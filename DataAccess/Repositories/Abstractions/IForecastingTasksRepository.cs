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

        Task AddForecastingTaskRecord(string taskName, List<ForecastingTaskFieldValue> values);

        Task AddBatchOfForecastingTaskRecords(string taskName, List<List<ForecastingTaskFieldValue>> values);

        Task DeleteForecastingTaskRecordById(string taskName, string recordId);

        Task<List<ForecastingTaskFieldDeclaration>> GetForecastingTaskFieldsDeclaration(string taskName);

        Task<bool> DoesForecastingTaskEntityExist(string taskName);
    }
}
