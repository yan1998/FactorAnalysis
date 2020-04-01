using AutoMapper;
using DataAccess.Repositories.Abstractions;
using DomainModel.ForecastingTasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Implementations
{
    public class ForecastingTasksMongoRepository : IForecastingTasksRepository
    {
        private readonly string _mongoDbConnectionString;
        private readonly string _databaseName;
        private readonly IMongoDatabase _database;
        private readonly IMapper _mapper;

        public ForecastingTasksMongoRepository(IConfiguration configuration,
            IMapper mapper)
        {
            _mongoDbConnectionString = configuration.GetConnectionString("MongoDbConnectionString");
            _databaseName = configuration["MongoDbName"];
            _database = new MongoClient(_mongoDbConnectionString).GetDatabase(_databaseName);
            _mapper = mapper;
        }

        public async Task<List<string>> GetAllForecastingTaskEntitiesName()
        {
            var allNames = await _database.ListCollectionNames().ToListAsync();
            return allNames.Where(x => !x.StartsWith("__")).ToList();
        }

        public async Task CreateForecastingTaskEntity(string taskName, List<DomainModel.ForecastingTasks.ForecastingTaskFactorDeclaration> declaration)
        {
            var forecastingTask = _mapper.Map<Model.ForecastingTaskDeclaration>(declaration);
            forecastingTask.Name = taskName;
            await _database.GetCollection<Model.ForecastingTaskDeclaration>("__declarations").InsertOneAsync(forecastingTask);
            await _database.CreateCollectionAsync(taskName);
        }

        public async Task DeleteForecastingTaskEntity(string taskName)
        {
            if (taskName.StartsWith("__"))
                throw new Exception("You cannot delete service collection");

            await _database.GetCollection<Model.ForecastingTaskDeclaration>("__declarations").DeleteOneAsync(x => x.Name == taskName);
            await _database.DropCollectionAsync(taskName);
        }

        public async Task<DomainModel.ForecastingTasks.ForecastingTask> GetForecastingTaskEntity(string taskName)
        {
            var taskDeclaration = await _database.GetCollection<Model.ForecastingTaskDeclaration>("__declarations").FindSync(x => x.Name == taskName).SingleAsync();
            var taskFactors = await _database.GetCollection<Model.ForecastingTaskFactorValues>(taskName).FindSync(x => true).ToListAsync();

            foreach (var taskFactor in taskFactors)
            {
                taskFactor.FactorsValue = taskFactor.FactorsValue.OrderBy(x => x.FactorId).ToList();
            }

            var domainObject = new DomainModel.ForecastingTasks.ForecastingTask
            {
                Name = taskName,
                FactorsDeclaration = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFactorDeclaration>>(taskDeclaration.FactorsDeclaration.OrderBy(x => x.Id)),
                FactorsValues = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFactorValues>>(taskFactors)
            };
            return domainObject;
        }

        public async Task<PagedForecastingTask> GetPagedForecastingTaskEntity(string taskName, int pageNumber, int perPage)
        {
            var taskDeclaration = await _database.GetCollection<Model.ForecastingTaskDeclaration>("__declarations").FindSync(x => x.Name == taskName).SingleAsync();
            var taskFactors = await _database.GetCollection<Model.ForecastingTaskFactorValues>(taskName).Find(x => true).Skip((pageNumber - 1) * perPage).Limit(perPage).ToListAsync();
            foreach (var taskFactor in taskFactors)
            {
                taskFactor.FactorsValue = taskFactor.FactorsValue.OrderBy(x => x.FactorId).ToList();
            }

            var pagedForecastingTask = new PagedForecastingTask
            {
                Name = taskName,
                FactorsDeclaration = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFactorDeclaration>>(taskDeclaration.FactorsDeclaration.OrderBy(x => x.Id)),
                FactorsValues = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFactorValues>>(taskFactors),
                TotalCount = await _database.GetCollection<Model.ForecastingTaskFactorValues>(taskName).CountDocumentsAsync(x => true)
            };
            return pagedForecastingTask;
        }

        public async Task AddForecastingTaskFactors(string taskName, List<DomainModel.ForecastingTasks.ForecastingTaskFactorValue> values)
        {
            var dataRecord = new Model.ForecastingTaskFactorValues
            {
                FactorsValue = _mapper.Map<List<Model.ForecastingTaskFactorValue>>(values)
            };
            
            await _database.GetCollection<Model.ForecastingTaskFactorValues>(taskName).InsertOneAsync(dataRecord);
        }

        public Task DeleteForecastingTaskFactorsById(string taskName, string id)
        {
            return _database.GetCollection<Model.ForecastingTaskFactorValues>(taskName).DeleteOneAsync(x => x._id == ObjectId.Parse(id));
        }

        public async Task<List<ForecastingTaskFactorDeclaration>> GetForecastingTaskFactorDeclaration(string taskName)
        {
            var taskDeclaration = await _database.GetCollection<Model.ForecastingTaskDeclaration>("__declarations").FindSync(x => x.Name == taskName).SingleAsync();
            return _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFactorDeclaration>>(taskDeclaration.FactorsDeclaration);
        }
    }
}
