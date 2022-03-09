using System.Diagnostics;
using Microsoft.Extensions.Primitives;
using Geev.Api.Framrwork.Tracing.Diagnostic;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace Geev.Api.Framrwork.Tracing.Http;

public readonly struct HttpDiagnosticSource
{
    private readonly ActivitySource _source;
    private static readonly TextMapPropagator InjectPropagator = Propagators.DefaultTextMapPropagator;
    private static readonly TextMapPropagator ExtractPropagator = new TraceContextPropagator();

    public HttpDiagnosticSource(ActivitySource source)
    {
        _source = source;
    }

    public void AddActivityToResponseHeader(IHeaderDictionary responseHeader)
    {
        if (Activity.Current != null)
            InjectPropagator.Inject(new PropagationContext(Activity.Current.Context, Baggage.Current),
                responseHeader, InjectTraceContextIntoHeaders);
    }

    public Activity? SetParentActivityFromRequestHeader(IHeaderDictionary requestHeader)
    {
        if (Activity.Current is null) return null;

        var parentContext = ExtractPropagator.Extract(default, requestHeader, ExtractTraceContextFromHeaders);
        // check if traceId is not in request headers
        if (int.TryParse(parentContext.ActivityContext.SpanId.ToString(), out var spanId)) return null;

        Baggage.Current = parentContext.Baggage;
        var currentActivity = Activity.Current;
        var activity = _source
            .StartActivity(currentActivity.DisplayName, ActivityKind.Internal, parentContext.ActivityContext);

        return activity;
    }

    private static IEnumerable<string> ExtractTraceContextFromHeaders(IHeaderDictionary headers, string key)
    {
        try
        {
            if (headers.TryGetValue($"{DiagnosticHeaders.TracingPrefix}{key}", out StringValues value))
            {
                return new[] { value.ToString() };
            }
        }
        catch (Exception ex)
        {
            // ignored
        }

        return Enumerable.Empty<string>();
    }

    private static void InjectTraceContextIntoHeaders(IHeaderDictionary headers, string key, string value)
    {
        try
        {
            headers.Append($"{DiagnosticHeaders.TracingPrefix}{key}", value);
        }
        catch (Exception ex)
        {
            // ignored
        }
    }
}
