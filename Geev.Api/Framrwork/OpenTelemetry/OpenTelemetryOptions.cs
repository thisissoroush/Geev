using Geev.Api.Framrwork.Tracing;
using StackExchange.Redis;

namespace Geev.Api.Framrwork.OpenTelemetry;

public class OpenTelemetryOptions
{
    public bool Enable { get; set; }
    public double TraceRatio { get; set; }
    public List<string> ExcludedPaths { get; set; } = OpenTelemetryDefaultSettings.GetDefaultExcludedPaths();
    public ZipkinOptions Zipkin { get; set; } = new();
    public TracingServices Services { get; set; } = new();
    public string ApplicationName { get; set; }
    public FilterActivityOptions FilterActivities { get; set; } = new();

    public class RedisOptions
    {
        public IConnectionMultiplexer ReadConnectionMultiplexer { get; set; }
        public IConnectionMultiplexer WriteConnectionMultiplexer { get; set; }
    }

    public class FilterActivityOptions
    {
        public List<string> DisplayNames { get; set; } = new();

        public List<KeyValuePair<string, string>> Tags { get; set; } = OpenTelemetryDefaultSettings.GetFilteredTags();

        public List<List<KeyValuePair<string, string>>> TagGroups { get; set; } = new();

        public List<List<KeyValuePair<string, string>>> TagGroupsWithoutParent { get; set; } =
            OpenTelemetryDefaultSettings.GetFilteredTagGroupsWithoutParent();
    }
    public class TracingServices
    {
        public RedisOptions Redis { get; set; } = new();
    }
    public class ZipkinOptions
    {
        public string Url { get; set; }
    }
}
