using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CodebridgeTest.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddScoped<Core.Interfaces.IPaginationService, Services.PaginationService>();
            services.AddScoped<Core.Interfaces.ISortService<Core.Models.Dog>, Services.DogSortService>();
            return services;
        }
    }
}
