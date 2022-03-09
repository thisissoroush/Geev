using System.Diagnostics;
using Geev.Api.Framrwork.Tracing.Diagnostic;

namespace Geev.Api.Framrwork.OpenTelemetry;

public static class IntFlightTracingActivitySource
{
    public const string Name = DiagnosticContext.MTRTracingActivitySource.Name;
    public static readonly ActivitySource Instance = DiagnosticContext.MTRTracingActivitySource.Instance;
}
