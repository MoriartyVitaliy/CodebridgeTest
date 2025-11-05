using CodebridgeTest.Application;
using CodebridgeTest.API.Middleware;
using CodebridgeTest.Persistence;
using CodebridgeTest.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace CodebridgeTest.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplication();
            builder.Services.AddPersistence(builder.Configuration);

            builder.Services.AddRedisRateLimiter(builder.Configuration);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRequestLogging();
            app.UseRedisRateLimiter();

            app.UseRouting();

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.UseGlobalExceptionHandler();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }

            app.Run();
        }
    }
}
