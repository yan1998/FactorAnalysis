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

        public async Task CreateForecastingTaskEntity(string taskName, List<DomainModel.ForecastingTasks.ForecastingTaskFieldDeclaration> declaration)
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
            var taskFactors = await _database.GetCollection<Model.ForecastingTaskFieldValues>(taskName).FindSync(x => true).ToListAsync();

            foreach (var taskFactor in taskFactors)
            {
                taskFactor.FactorsValue = taskFactor.FactorsValue.OrderBy(x => x.FieldId).ToList();
            }

            var domainObject = new DomainModel.ForecastingTasks.ForecastingTask
            {
                Name = taskName,
                FieldsDeclaration = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFieldDeclaration>>(taskDeclaration.FieldsDeclaration.OrderBy(x => x.Id)),
                FactorsValues = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFieldValues>>(taskFactors)
            };
            return domainObject;
        }

        public async Task<PagedForecastingTask> GetPagedForecastingTaskEntity(string taskName, int pageNumber, int perPage)
        {
            var taskDeclaration = await _database.GetCollection<Model.ForecastingTaskDeclaration>("__declarations").FindSync(x => x.Name == taskName).SingleAsync();
            var taskFactors = await _database.GetCollection<Model.ForecastingTaskFieldValues>(taskName).Find(x => true).Skip((pageNumber - 1) * perPage).Limit(perPage).ToListAsync();
            foreach (var taskFactor in taskFactors)
            {
                taskFactor.FactorsValue = taskFactor.FactorsValue.OrderBy(x => x.FieldId).ToList();
            }

            var pagedForecastingTask = new PagedForecastingTask
            {
                Name = taskName,
                FieldsDeclaration = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFieldDeclaration>>(taskDeclaration.FieldsDeclaration.OrderBy(x => x.Id)),
                FactorsValues = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFieldValues>>(taskFactors),
                TotalCount = await _database.GetCollection<Model.ForecastingTaskFieldValues>(taskName).CountDocumentsAsync(x => true)
            };
            return pagedForecastingTask;
        }

        public async Task AddForecastingTaskFactors(string taskName, List<DomainModel.ForecastingTasks.ForecastingTaskFieldValue> values)
        {
            var dataRecord = new Model.ForecastingTaskFieldValues
            {
                FactorsValue = _mapper.Map<List<Model.ForecastingTaskFieldValue>>(values)
            };
            
            await _database.GetCollection<Model.ForecastingTaskFieldValues>(taskName).InsertOneAsync(dataRecord);
        }

        public Task DeleteForecastingTaskFactorsById(string taskName, string id)
        {
            return _database.GetCollection<Model.ForecastingTaskFieldValues>(taskName).DeleteOneAsync(x => x._id == ObjectId.Parse(id));
        }

        public async Task<List<ForecastingTaskFieldDeclaration>> GetForecastingTaskFieldDeclaration(string taskName)
        {
            var taskDeclaration = await _database.GetCollection<Model.ForecastingTaskDeclaration>("__declarations").FindSync(x => x.Name == taskName).SingleAsync();
            return _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFieldDeclaration>>(taskDeclaration.FieldsDeclaration);
        }
    }
}
