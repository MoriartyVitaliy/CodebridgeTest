using CodebridgeTest.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace CodebridgeTest.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        private static readonly Dictionary<Type, (HttpStatusCode, LogLevel, string)> ExceptionMappings = new()
        {
            { typeof(ValidationException), (HttpStatusCode.BadRequest, LogLevel.Warning, "Validation error") },
            { typeof(ResourceNotFoundException), (HttpStatusCode.NotFound, LogLevel.Warning, "Resource not found") },
            { typeof(RateLimitExceededException), ((HttpStatusCode)429, LogLevel.Warning, "Too many requests") },
            { typeof(Exception), (HttpStatusCode.InternalServerError, LogLevel.Error, "Unexpected server error") }
        };

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;

        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var (statusCode, level, defaultMessage) = ExceptionMappings.TryGetValue(ex.GetType(), out var mapping)
                    ? mapping
                    : (HttpStatusCode.InternalServerError, LogLevel.Error, "Unexpected server error");

                _logger.Log(level, ex, defaultMessage);
                await HandleExceptionAsync(context, statusCode, ex.Message, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message, Exception ex)
        {
            object response = new { error = message, details = ex.ToString(), traceId = context.TraceIdentifier };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(json);
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
