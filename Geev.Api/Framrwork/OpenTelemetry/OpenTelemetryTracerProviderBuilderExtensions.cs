using OpenTelemetry.Trace;

namespace Geev.Api.Framrwork.OpenTelemetry;

internal static class OpenTelemetryTracerProviderBuilderExtensions
{
    internal static TracerProviderBuilder AddRedisInstrumentationIfExist(this TracerProviderBuilder tracerProviderBuilder
        , OpenTelemetryOptions.RedisOptions redisOptions)
    {

        if (redisOptions.WriteConnectionMultiplexer is null || redisOptions.ReadConnectionMultiplexer is null)
            return tracerProviderBuilder;

        tracerProviderBuilder
            .AddRedisInstrumentation(redisOptions.ReadConnectionMultiplexer)
            .AddRedisInstrumentation(redisOptions.WriteConnectionMultiplexer);

        return tracerProviderBuilder;
    }

}
