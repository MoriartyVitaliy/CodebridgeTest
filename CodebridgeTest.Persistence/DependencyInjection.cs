using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using CodebridgeTest.Persistence.Data;
using CodebridgeTest.Persistence.Data.Repositories;
using CodebridgeTest.Core.Interfaces;

namespace CodebridgeTest.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSql")));

            services.AddScoped<IDogRepository, DogRepository>();

            return services;
        }
    }
}
