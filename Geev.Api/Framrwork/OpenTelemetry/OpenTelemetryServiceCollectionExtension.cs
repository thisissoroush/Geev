using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using HttpMethod = System.Net.Http.HttpMethod;

namespace Geev.Api.Framrwork.OpenTelemetry;

internal static class OpenTelemetryServiceCollectionExtension
{
    internal static IServiceCollection AddOpenTelemetryIfEnabled(this IServiceCollection services, OpenTelemetryOptions openTelemetryOptions)
    {
        if (openTelemetryOptions.Enable)
        {
            services.AddOpenTelemetryTracing(builder =>
            {
                builder.Configure((sp, traceProviderBuilder) =>
                {
                    var zipkinExporter = new ZipkinExporter(new ZipkinExporterOptions
                    {
                        Endpoint = new Uri(openTelemetryOptions.Zipkin.Url),
                        MaxPayloadSizeInBytes = 1 << 17
                    });

                    //SetRedisInstrumentationConfig(openTelemetryOptions, sp);

                    traceProviderBuilder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(openTelemetryOptions.ApplicationName))
                    .AddAspNetCoreInstrumentation(opt =>
                    {
                        opt.RecordException = true;
                        opt.Filter = httpContext =>
                        {
                            return !openTelemetryOptions.ExcludedPaths.Any(excludedPath =>
                                httpContext.Request.Path.StartsWithSegments(new PathString(excludedPath)));
                        };
                    })
                    .AddHttpClientInstrumentation(opt =>
                    {
                        opt.RecordException = true;
                        opt.Filter = httpRequestMessage => httpRequestMessage.Method != HttpMethod.Head;
                        opt.SetHttpFlavor = true;
                    })
                    .AddSource(IntFlightTracingActivitySource.Name)
                    .AddSqlClientInstrumentation(opt =>
                    {
                        opt.SetDbStatementForText = true;
                        opt.SetDbStatementForStoredProcedure = false;
                        opt.RecordException = true;
                        opt.EnableConnectionLevelAttributes = true;
                    })
                    .AddRedisInstrumentationIfExist(openTelemetryOptions.Services.Redis)
                    .AddMassTransitInstrumentation()
                    .AddProcessor(new CustomActivityFilterProcessor(zipkinExporter, openTelemetryOptions.FilterActivities))
                    .SetSampler(new TraceIdRatioBasedSampler(openTelemetryOptions.TraceRatio));
                });
            });
        }

        return services;
    }

    //private static void SetRedisInstrumentationConfig(OpenTelemetryOptions openTelemetryOptions, IServiceProvider sp)
    //{
    //    openTelemetryOptions.Services.Redis.ReadConnectionMultiplexer = sp.GetRequiredService<RedisDatabaseResolver>()?.GetReadConnectionMultiplexer();
    //    openTelemetryOptions.Services.Redis.WriteConnectionMultiplexer = sp.GetRequiredService<RedisDatabaseResolver>()?.GetWriteConnectionMultiplexer();
    //}
}

