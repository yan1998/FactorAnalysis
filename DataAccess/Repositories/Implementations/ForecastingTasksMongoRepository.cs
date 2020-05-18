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
                Description = taskDeclaration.Description,
                FieldsDeclaration = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFieldDeclaration>>(taskDeclaration.FieldsDeclaration.OrderBy(x => x.Id)),
                Records = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskRecord>>(taskfields)
            };
            return domainObject;
        }

        public async Task<PagedForecastingTask> SearchForecastingTaskRecords(SearchForecastingTaskRecords searchRequest)
        {
            var taskFields = await _database.GetCollection<Model.ForecastingTaskFieldValues>(searchRequest.TaskEntityName).Find(x => true).ToListAsync();

            var filteredRecords = new List<Model.ForecastingTaskFieldValues>();
            if (searchRequest.Filters?.Count != 0)
            {
                foreach (var filter in searchRequest.Filters.Distinct())
                {
                    filteredRecords.AddRange(taskFields.Where(x => x.FieldsValue.Any(x => x.FieldId == filter.FieldId && x.Value.ToLower() == filter.Value.ToLower())));
                }
            }
            else
            {
                filteredRecords = taskFields;
            }
            
            foreach (var taskRecord in filteredRecords)
            {
                taskRecord.FieldsValue = taskRecord.FieldsValue.OrderBy(x => x.FieldId).ToList();
            }

            var pagedForecastingTask = new PagedForecastingTask
            {
                Name = searchRequest.TaskEntityName,
                Records = _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskRecord>>(filteredRecords.Skip((searchRequest.PageNumber - 1) * searchRequest.PerPage).Take(searchRequest.PerPage)),
                TotalCount = filteredRecords.Count
            };
            return pagedForecastingTask;
        }

        public async Task AddForecastingTaskRecord(string taskName, List<DomainModel.ForecastingTasks.ForecastingTaskFieldValue> values)
        {
            var dataRecord = new Model.ForecastingTaskFieldValues
            {
                FieldsValue = _mapper.Map<List<Model.ForecastingTaskFieldValue>>(values)
            };
            
            await _database.GetCollection<Model.ForecastingTaskFieldValues>(taskName).InsertOneAsync(dataRecord);
        }

        public async Task AddBatchOfForecastingTaskRecords(string taskName, List<List<ForecastingTaskFieldValue>> values)
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

        public Task DeleteForecastingTaskRecordById(string taskName, string recordId)
        {
            return _database.GetCollection<Model.ForecastingTaskFieldValues>(taskName).DeleteOneAsync(x => x._id == ObjectId.Parse(recordId));
        }

        public async Task<List<ForecastingTaskFieldDeclaration>> GetForecastingTaskFieldsDeclaration(string taskName)
        {
            var taskDeclaration = await _database.GetCollection<Model.ForecastingTaskDeclaration>("__declarations").FindSync(x => x.Name == taskName).SingleAsync();
            return _mapper.Map<List<DomainModel.ForecastingTasks.ForecastingTaskFieldDeclaration>>(taskDeclaration.FieldsDeclaration.OrderBy(x => x.Id));
        }

        public async Task<bool> DoesForecastingTaskEntityExist(string taskName)
        {
            var existingEntityName = await _database.GetCollection<Model.ForecastingTaskDeclaration>("__declarations").FindAsync(x => x.Name.ToLower() == taskName.ToLower());
            return await existingEntityName.AnyAsync();
        }
    }
}
