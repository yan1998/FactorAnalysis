using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FactorAnalysis.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["ConnectionStrings:SqlConnectionString"];
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));

            // services.AddScoped<ICallsRepository, CallsRepository>();
        }

    }
}
