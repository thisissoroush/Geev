using System.Diagnostics;

namespace Geev.Api.Framrwork.Tracing.Diagnostic;

public static class DiagnosticContext
{
    public static bool GetEnabled()
    {
        return MTRTracingActivitySource.Instance.HasListeners();
    }

    public static class MTRTracingActivitySource
    {
        public const string Name = "MTRTracingActivitySource";
        public static readonly ActivitySource Instance = new(Name);
    }

}
