using DomainModel.ForecastingTasks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Abstractions
{
    public interface IForecastingTasksService
    {
        Task<List<ShortForecastingTaskInfo>> GetAllForecastingTaskEntities();

        Task CreateForecastingTaskEntity(string entityName, string description, List<ForecastingTaskFieldDeclaration> declaration);

        Task EditForecastingTaskEntity(string oldTaskName, string newTaskName, string newTaskDescription);

        Task DeleteForecastingTaskEntity(string entityName);

        Task AddForecastingTaskRecord(string entityName, List<ForecastingTaskFieldValue> fields);

        Task DeleteForecastingTaskRecordById(string entityName, string recordId);

        Task<List<ForecastingTaskFieldDeclaration>> GetForecastingTaskEntityDeclaration(string entityName);

        Task<PagedForecastingTask> SearchForecastingTaskRecords(SearchForecastingTaskRecords searchRequest);
    }
}
