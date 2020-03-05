using DomainModel.ForecastingTasks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Abstractions
{
    public interface IForecastingTasksService
    {
        Task<List<string>> GetAllForecastingTaskEntitiesName();

        Task CreateForecastingTaskEntity(string entityName, List<ForecastingTaskFactorDeclaration> declaration);

        Task DeleteForecastingTaskEntity(string entityName);

        Task AddForecastingTaskFactors(string entityName, List<ForecastingTaskFactorValue> values);

        Task DeleteForecastingTaskFactorsById(string entityName, string id);

        Task<PagedForecastingTask> GetPagedForecastingTaskEntity(string entityName, int pageNumber, int perPage);
    }
}
