using Geev.Api;
using Serilog;
using Serilog.Formatting.Elasticsearch;

public class Program
{
    public async static Task Main(string[] args)
    {
        try
        {
            var host = CreateHostBuilder(args).Build();

            await host.RunAsync();
        }
        catch (Exception)
        {
            Environment.Exit(-1);
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>()
                .ConfigureLogging((_, logging) => { logging.ClearProviders(); })
                        .UseSerilog((ctx, cfg) =>
                        {
                            cfg.ReadFrom.Configuration(ctx.Configuration)
                                .Enrich.FromLogContext()
                                .Enrich.WithProperty("service_name", nameof(Geev.Api));

                            if (ctx.HostingEnvironment.IsDevelopment())
                            {
                                cfg.WriteTo.Async(sinkCfg => sinkCfg.Console());
                            }
                            else
                            {
                                cfg.WriteTo.Async(sinkCfg => sinkCfg.Console(new ElasticsearchJsonFormatter()));
                            }
                        });
            }
                );
}