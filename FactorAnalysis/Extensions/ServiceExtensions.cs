using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DataAccess.Repositories.Abstractions;
using DataAccess.Repositories.Implementations;
using BusinessLogic.Services.Abstractions;
using BusinessLogic.Services.Implementations;

namespace FactorAnalysis.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["ConnectionStrings:SqlConnectionString"];
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IExchangeRateFactorsRepository, ExchangeRateFactorsRepository>();
        }

        public static void ConfigureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ISeedExchangeRateFactorsService, SeedExchangeRateFactorsService>();
            services.AddScoped<IExchangeRateFactorsService, ExchangeRateFactorsService>();
        }
    }
}
