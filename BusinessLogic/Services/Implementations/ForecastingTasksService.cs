using BusinessLogic.Exceptions;
using BusinessLogic.Services.Abstractions;
using DataAccess.Repositories.Abstractions;
using DomainModel.ForecastingTasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Implementations
{
    public class ForecastingTasksService : IForecastingTasksService
    {
        private readonly IForecastingTasksRepository _forecastingTasksRepository;

        public ForecastingTasksService(IForecastingTasksRepository forecastingTasksRepository)
        {
            _forecastingTasksRepository = forecastingTasksRepository;
        }

        public Task<List<string>> GetAllForecastingTaskEntitiesName()
        {
            return _forecastingTasksRepository.GetAllForecastingTaskEntitiesName();
        }

        public async Task CreateForecastingTaskEntity(string entityName, List<ForecastingTaskFactorDeclaration> declaration)
        {
            if(declaration.Count(x => x.IsPredicatedValue) != 1)
                throw new DomainErrorException($"Forecasting task must to have one predicated value!");

            if (await DoesForecastingTaskEntityExists(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} already exists!");

            int i = 0;
            foreach (var declarationItem in declaration)
            {
                declarationItem.Id = i++;
            }
            await _forecastingTasksRepository.CreateForecastingTaskEntity(entityName, declaration);
        }

        public async Task DeleteForecastingTaskEntity(string entityName)
        {
            if(!await DoesForecastingTaskEntityExists(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            await _forecastingTasksRepository.DeleteForecastingTaskEntity(entityName);
        }

        public async Task AddForecastingTaskFactors(string entityName, List<ForecastingTaskFactorValue> values)
        {
            if (!await DoesForecastingTaskEntityExists(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            await _forecastingTasksRepository.AddForecastingTaskFactors(entityName, values);
        }

        public async Task DeleteForecastingTaskFactorsById(string entityName, string id)
        {
            if (!await DoesForecastingTaskEntityExists(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            await _forecastingTasksRepository.DeleteForecastingTaskFactorsById(entityName, id);
        }

        public async Task<PagedForecastingTask> GetPagedForecastingTaskEntity(string entityName, int pageNumber, int perPage)
        {
            if (pageNumber <= 0)
                throw new DomainErrorException("Page number should be greater than 0!");
            if (perPage <= 0)
                throw new DomainErrorException("Per page amount should be greater than 0!");
            if(string.IsNullOrWhiteSpace(entityName))
                throw new DomainErrorException($"Forecasting task name must to be filled!");
            if (!await DoesForecastingTaskEntityExists(entityName))
                throw new DomainErrorException($"Forecasting task with name {entityName} doesn't exist!");

            return await _forecastingTasksRepository.GetPagedForecastingTaskEntity(entityName, pageNumber, perPage);
        }

        private async Task<bool> DoesForecastingTaskEntityExists(string entityName)
        {
            var existingEntityNames = await _forecastingTasksRepository.GetAllForecastingTaskEntitiesName();
            return existingEntityNames.Contains(entityName);
        }
    }
}
