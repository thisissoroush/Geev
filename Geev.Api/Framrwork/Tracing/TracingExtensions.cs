using zipkin4net;

namespace Geev.Api.Framrwork.Tracing;

public static class TracingExtensions
{

    /// <summary>
    /// Returns the string representation of trace correlation Id.
    /// </summary>
    /// <returns></returns>
    public static string GetCompactTraceId()
    {
        return Trace.Current != null
            ? Trace.Current.CorrelationId.ToString().Replace("-", "").TrimStart('0').ToLower()
            : "empty";
    }
}
