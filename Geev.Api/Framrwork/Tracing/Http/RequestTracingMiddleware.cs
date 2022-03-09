using System.Diagnostics;

namespace Geev.Api.Framrwork.Tracing.Http;

public class RequestTracingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestTracingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        HttpActivityManager.AddActivityToResponseHeader(context.Response.Headers);
        await _next(context);
        Activity.Current?.Stop();
    }
}
