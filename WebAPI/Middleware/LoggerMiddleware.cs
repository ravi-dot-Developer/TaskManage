using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

public class LoggerMiddleware : IMiddleware
{
    private readonly ILogger<LoggerMiddleware> _logger;

    public LoggerMiddleware(ILogger<LoggerMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _logger.LogInformation($"Received request: {context.Request.Method} {context.Request.Path}");

        var stopwatch = Stopwatch.StartNew();

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();
            // Log information about outgoing response
            _logger.LogInformation($"Processed request: {context.Request.Method} {context.Request.Path} => {context.Response.StatusCode} in {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
