namespace Geev.Api.Framrwork.Tracing.Diagnostic;

public static class DiagnosticHeaders
{
    public const string TracingPrefix = "mtr-";
    public static readonly string ParentTrace = $"{TracingPrefix}traceparent";
    public static readonly string TraceId = $"{TracingPrefix}traceid";
}
