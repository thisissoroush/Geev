using Microsoft.Extensions.Options;
using Geev.Api.Framrwork.Metadata;

namespace Geev.Api.Framrwork.Tracing;

public class ServiceInformationTracingMiddleware : IMiddleware
{
    public const string RequesterServiceHeaderName = "mtr-req-service";
    public const string RequesterServiceVersionHeaderName = "mtr-req-service-version";

    public const string ResponseServiceHeaderName = "mtr-resp-service";
    public const string ResponseServiceVersionHeaderName = "mtr-resp-service-version";

    private readonly ServiceInformation _serviceInfo;

    public ServiceInformationTracingMiddleware(IOptions<ServiceInformation> options)
    {
        _serviceInfo = options.Value;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // Adds this information to the response
        context.Response.Headers.TryAdd(ResponseServiceHeaderName, _serviceInfo.Name);
        context.Response.Headers.TryAdd(ResponseServiceVersionHeaderName, _serviceInfo.Version);

        context.Items.Add(RequesterServiceHeaderName, _serviceInfo.Name);
        context.Items.Add(RequesterServiceVersionHeaderName, _serviceInfo.Version);
        await next(context);
    }
}
