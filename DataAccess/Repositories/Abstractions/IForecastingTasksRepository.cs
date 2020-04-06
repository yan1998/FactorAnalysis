using DomainModel.ForecastingTasks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Abstractions
{
    public interface IForecastingTasksRepository
    {
        Task<List<string>> GetAllForecastingTaskEntitiesName();

        Task CreateForecastingTaskEntity(string taskName, List<ForecastingTaskFieldDeclaration> declaration);

        Task DeleteForecastingTaskEntity(string taskName);

        Task<ForecastingTask> GetForecastingTaskEntity(string taskName);

        Task<PagedForecastingTask> GetPagedForecastingTaskEntity(string taskName, int pageNumber, int perPage);

        Task AddForecastingTaskFactors(string taskName, List<ForecastingTaskFieldValue> values);

        Task DeleteForecastingTaskFactorsById(string taskName, string id);

        Task<List<ForecastingTaskFieldDeclaration>> GetForecastingTaskFieldDeclaration(string taskName);
    }
}
