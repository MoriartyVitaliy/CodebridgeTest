using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace CodebridgeTest.Middleware
{
    public class RedisRateLimitOptions
    {
        public int RequestsPerSecond { get; set; } = 10;
    }

    public class RedisRateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConnectionMultiplexer _redis;
        private readonly int _requestsPerSecond;
        private readonly ILogger<RedisRateLimitingMiddleware> _logger;

        public RedisRateLimitingMiddleware(RequestDelegate next,
                                           IConnectionMultiplexer redis,
                                           IOptions<RedisRateLimitOptions> options,
                                           ILogger<RedisRateLimitingMiddleware> logger)
        {
            _next = next;
            _redis = redis;
            _logger = logger;
            _requestsPerSecond = options.Value.RequestsPerSecond;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var db = _redis.GetDatabase();


            var key = $"ratelimit:{DateTime.UtcNow:yyyyMMddHHmmss}";

            var count = await db.StringIncrementAsync(key);

            if (count == 1)
                await db.KeyExpireAsync(key, TimeSpan.FromSeconds(1));

            if (count > _requestsPerSecond)
            {
                _logger.LogWarning("Rate limit exceeded. Count={Count}, Limit={Limit}", count, _requestsPerSecond);
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Too many requests. Please try again later.");
                return;
            }

            await _next(context);
        }
    }

    public static class RedisRateLimitExtensions
    {
        public static IServiceCollection AddRedisRateLimiter(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<RedisRateLimitOptions>(config.GetSection("RateLimiting"));
            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(config.GetConnectionString("Redis")!));
            return services;
        }

        public static IApplicationBuilder UseRedisRateLimiter(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RedisRateLimitingMiddleware>();
        }
    }
}
