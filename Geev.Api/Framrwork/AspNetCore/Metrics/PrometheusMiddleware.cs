namespace Geev.Api.Framrwork.AspNetCore.Metrics;

public class PrometheusMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public PrometheusMiddleware(RequestDelegate next, ILogger<PrometheusMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var path = httpContext.Request.Path;
        var method = httpContext.Request.Method;

        //var counter = Prometheus.Metrics.CreateCounter("dotnet_http_requests_total", "HTTP Requests Total",
        //    labelNames: new[] {"path", "method", "status"});
        var countCounter = Prometheus.Metrics.CreateCounter("dotnet_http_requests_total_count", "HTTP Requests Total",
            labelNames: new string[0]);
        var statusCode = 200;
        try
        {
            await _next.Invoke(httpContext);
        }
        catch (Exception)
        {
            statusCode = 500;
            //counter.Labels(path, method, statusCode.ToString()).Inc();
            countCounter.Inc();
            throw;
        }

        if (path.ToString().StartsWith("/api"))
        {
            statusCode = httpContext.Response.StatusCode;
            //counter.Labels(path, method, statusCode.ToString()).Inc();
        }
        countCounter.Inc();
    }
}

public static class PrometheusMiddlewareExtensions
{
    public static IApplicationBuilder UsePrometheusMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<PrometheusMiddleware>();
    }
}
