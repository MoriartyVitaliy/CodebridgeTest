using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using CodebridgeTest.Persistence.Data;
using CodebridgeTest.Persistence.Data.Repositories;

namespace CodebridgeTest.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            //services.AddScoped<IDogRepository, DogRepository>();

            return services;
        }
    }
}
