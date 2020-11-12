using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DataAccess.Repositories.Abstractions;
using DataAccess.Repositories.Implementations;
using BusinessLogic.Services.Abstractions;
using BusinessLogic.Services.Implementations;
using AutoMapper;
using FactorAnalysis.Mappers;

namespace FactorAnalysis.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureRepositories(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["ConnectionStrings:SqlConnectionString"];
            services.AddScoped<IForecastingTasksRepository, ForecastingTasksMongoRepository>();
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IForecastingTasksService, ForecastingTasksService>();
            services.AddScoped<IMachineLearningService, MachineLearningService>();
            services.AddScoped<IImportExportInFileService, ImportExportInFileService>();
        }

        public static void AddContractMappings(this IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.AddProfile<ContractToBusinnessProfile>();
            mapperConfigurationExpression.AddProfile<BusinnessToContractProfile>();
        }
    }
}
