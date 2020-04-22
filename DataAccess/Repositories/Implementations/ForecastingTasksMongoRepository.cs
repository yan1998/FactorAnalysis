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

        public async Task<List<ShortForecastingTaskInfo>> GetAllForecastingTaskEntities()
        {
            var taskDeclaration = await _database.GetCollection<Model.ForecastingTaskDeclaration>("__declarations").FindAsync(x => true);
            return _mapper.Map<List<ShortForecastingTaskInfo>>(taskDeclaration.ToList().OrderBy(x => x.Name));
        }

        public async Task CreateForecastingTaskEntity(string taskName, string description, List<DomainModel.ForecastingTasks.ForecastingTaskFieldDeclaration> declaration)
        {
            var forecastingTask = _mapper.Map<Model.ForecastingTaskDeclaration>(declaration);
            forecastingTask.Name = taskName;
            forecastingTask.Description = description;
            await _database.GetCollection<Model.ForecastingTaskDeclaration>("__declarations").InsertOneAsync(forecastingTask);
            await _database.CreateCollectionAsync(taskName);
        }

        public async Task EditForecastingTaskEntity(string oldTaskName, string newTaskName, string newTaskDescription)
        {
            var arrayFilter = Builders<Model.ForecastingTaskDeclaration>.Filter.Eq("Name", oldTaskName);
            var arrayUpdate = Builders<Model.ForecastingTaskDeclaration>.Update.Set("Name", newTaskName).Set("Description", newTaskDescription);

            await _database.GetCollection<Model.ForecastingTaskDeclaration>("__declarations").UpdateOneAsync(arrayFilter, arrayUpdate);
            if(oldTaskName != newTaskName)
                await _database.RenameCollectionAsync(oldTaskName, newTaskName);
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
            var taskfields = await _database.GetCollection<Model.ForecastingTaskFieldValues>(taskName).FindSync(x => true).ToListAsync();

            foreach (var taskField in taskfields)
            {
                taskField.FieldsValue = taskField.FieldsValue.OrderBy(x => x.FieldId).ToList();
            }

            var domainObject = new DomainModel.ForecastingTasks.ForecastingTask
            {
                Name = taskName,
                FieldsDeclaration = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFieldDeclaration>>(taskDeclaration.FieldsDeclaration.OrderBy(x => x.Id)),
                FieldsValues = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFieldValues>>(taskfields)
            };
            return domainObject;
        }

        public async Task<PagedForecastingTask> GetPagedForecastingTaskEntity(string taskName, int pageNumber, int perPage)
        {
            var taskDeclaration = await _database.GetCollection<Model.ForecastingTaskDeclaration>("__declarations").FindSync(x => x.Name == taskName).SingleAsync();
            var taskFields = await _database.GetCollection<Model.ForecastingTaskFieldValues>(taskName).Find(x => true).Skip((pageNumber - 1) * perPage).Limit(perPage).ToListAsync();
            foreach (var taskField in taskFields)
            {
                taskField.FieldsValue = taskField.FieldsValue.OrderBy(x => x.FieldId).ToList();
            }

            var pagedForecastingTask = new PagedForecastingTask
            {
                Name = taskName,
                FieldsDeclaration = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFieldDeclaration>>(taskDeclaration.FieldsDeclaration.OrderBy(x => x.Id)),
                FieldsValues = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFieldValues>>(taskFields),
                TotalCount = await _database.GetCollection<Model.ForecastingTaskFieldValues>(taskName).CountDocumentsAsync(x => true)
            };
            return pagedForecastingTask;
        }

        public async Task AddForecastingTaskFields(string taskName, List<DomainModel.ForecastingTasks.ForecastingTaskFieldValue> values)
        {
            var dataRecord = new Model.ForecastingTaskFieldValues
            {
                FieldsValue = _mapper.Map<List<Model.ForecastingTaskFieldValue>>(values)
            };
            
            await _database.GetCollection<Model.ForecastingTaskFieldValues>(taskName).InsertOneAsync(dataRecord);
        }

        public async Task AddBatchOfForecastingTaskFields(string taskName, List<List<ForecastingTaskFieldValue>> values)
        {
            var dataRecords = new List<Model.ForecastingTaskFieldValues>();
            values.ForEach(x =>
            {
                dataRecords.Add(new Model.ForecastingTaskFieldValues
                {
                    FieldsValue = _mapper.Map<List<Model.ForecastingTaskFieldValue>>(x)
                });
            });

            await _database.GetCollection<Model.ForecastingTaskFieldValues>(taskName).InsertManyAsync(dataRecords);
        }

        public Task DeleteForecastingTaskFieldsById(string taskName, string id)
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
