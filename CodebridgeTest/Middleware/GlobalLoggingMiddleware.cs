using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text;

namespace CodebridgeTest.Middleware
{
    public class RequestLoggingOptions
    {
        public bool LogRequestBody { get; set; } = true;
        public bool LogResponseBody { get; set; } = true;
        public int MaxBodyLength { get; set; } = 2048;
    }

    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private readonly RequestLoggingOptions _options;

        public RequestLoggingMiddleware(RequestDelegate next,
                                        ILogger<RequestLoggingMiddleware> logger,
                                        IOptions<RequestLoggingOptions> options)
        {
            _next = next;
            _logger = logger;
            _options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var request = await FormatRequest(context.Request);
            _logger.LogInformation("Incoming Request: {Method} {Path}{Query}\n{Headers}\n{Body}",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString,
                request.Headers,
                request.Body);

            var originalBody = context.Response.Body;
            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            stopwatch.Stop();

            var response = await FormatResponse(context.Response);
            _logger.LogInformation("Outgoing Response ({Elapsed} ms): {StatusCode}\n{Headers}\n{Body}",
                stopwatch.ElapsedMilliseconds,
                context.Response.StatusCode,
                response.Headers,
                response.Body);

            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBody);
        }

        private async Task<(string Headers, string Body)> FormatRequest(HttpRequest request)
        {
            var headers = string.Join(", ", request.Headers.Select(h => $"{h.Key}: {h.Value}"));
            string body = "";

            if (_options.LogRequestBody && request.ContentLength > 0)
            {
                request.EnableBuffering();
                using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
                var text = await reader.ReadToEndAsync();
                request.Body.Position = 0;
                body = TruncateBody(text);
            }

            return (headers, body);
        }

        private async Task<(string Headers, string Body)> FormatResponse(HttpResponse response)
        {
            var headers = string.Join(", ", response.Headers.Select(h => $"{h.Key}: {h.Value}"));
            string body = "";

            if (_options.LogResponseBody)
            {
                response.Body.Seek(0, SeekOrigin.Begin);
                var text = await new StreamReader(response.Body).ReadToEndAsync();
                response.Body.Seek(0, SeekOrigin.Begin);
                body = TruncateBody(text);
            }

            return (headers, body);
        }

        private string TruncateBody(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            return text.Length <= _options.MaxBodyLength
                ? text
                : text[.._options.MaxBodyLength] + "...(truncated)";
        }
    }

    public static class RequestLoggingExtensions
    {
        public static IServiceCollection AddRequestLogging(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<RequestLoggingOptions>(config.GetSection("RequestLogging"));
            return services;
        }

        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
