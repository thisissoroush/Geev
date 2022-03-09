using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Geev.Api.Framrwork.AspNetCore.Config;
using Geev.Api.Framrwork.AspNetCore.Metrics;
using Geev.Api.Framrwork.AspNetCore.Mvc.ExceptionHandling;
using Geev.Api.Framrwork.AspNetCore.Mvc.Results;
using Geev.Api.Framrwork.AspNetCore.Mvc.Results.Wrapping;
using Geev.Api.Framrwork.Models;
using Geev.Api.Framrwork.OpenTelemetry;
using Geev.Api.Framrwork.Tracing;
using Geev.Api.Framrwork.Tracing.Http;
using Geev.Core;
using Geev.Data;
using Geev.Services;
using Geev.Services.Behaviours;
using Geev.Services.Commands;
using Geev.Services.Dapper;
using Prometheus;
using System.Reflection;

namespace Geev.Api;

public class Startup
{

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        Environment = env;
    }

    public IConfiguration Configuration { get; }

    public IWebHostEnvironment Environment { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        Console.WriteLine(Environment.EnvironmentName);

        // ASP.NET Core & 3rd parties
        services.AddControllers(options =>
        {
            options.Filters.AddService(typeof(GlobalExceptionFilter));
            options.Filters.AddService(typeof(ResultFilter));
        });
        services.AddCors();
        services.AddHttpContextAccessor();
        services.AddAutoMapper(typeof(DefaultMappingProfile).Assembly);
        services.AddHealthChecks();

        services.AddHttpContextAccessor();

        services.AddApiVersioning(setup =>
        {
            setup.DefaultApiVersion = new ApiVersion(1, 0);
            setup.AssumeDefaultVersionWhenUnspecified = true;
            setup.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });

        //Fluent Validation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        //MediatR
        services.AddMediatR(typeof(PingCommand));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        // Swagger
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(AppConsts.ApiVersion, new() { Title = AppConsts.ApiTitle, Version = AppConsts.ApiVersion });
            //uncomment for v2
            //options.SwaggerDoc("v2", new() { Title = AppConsts.ApiTitle, Version = "v2" });

            options.AddSecurityDefinition("Bearer", new()
            {
                Description =
                "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
        });

        //db context
        services.AddDbContext<GeevDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        //Dapper
        services.AddSingleton<IDbConnector, DbConnector>();
        services.AddSingleton(typeof(IDapper<>), typeof(Dapper<>));

        // Configure options
        services.Configure<BusConfig>(Configuration.GetSection("EventBus"));
        services.Configure<OpenTelemetryOptions>(Configuration.GetSection("OpenTelemetry"));
        services.Configure<Settings>(Configuration);
        

        //Setting up framework
        //Adds services required for using options.
        services.AddOptions();
        services.AddCors();

        // Add Geev to DI
        services.AddTransient<ResultFilter>();
        services.AddTransient<IActionResultWrapperFactory, ActionResultWrapperFactory>();
        services.AddTransient<IAspnetCoreConfiguration, AspnetCoreConfiguration>();
        services.AddTransient<IErrorInfoBuilder, ErrorInfoBuilder>();
        services.AddTransient<GlobalExceptionFilter>();
        services.AddTransient<ServiceInformationTracingMiddleware>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddSingleton(Configuration);

        var openTelemtryOptions = Configuration.GetSection("OpenTelemetry").Get<OpenTelemetryOptions>();
        openTelemtryOptions.ApplicationName = AppConsts.AppName;
        services.AddOpenTelemetryIfEnabled(openTelemtryOptions);

        services.AddMemoryCache();
    }

    public void Configure(IApplicationBuilder app,
        IWebHostEnvironment env,
        IMapper mapper,
        IApiVersionDescriptionProvider provider)
    {

        mapper.ConfigurationProvider.AssertConfigurationIsValid();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseMetricServer();
        app.UsePrometheusMiddleware();
        app.UseRequestTracing();


        app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResultStatusCodes =
                {
                        [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded] = StatusCodes.Status503ServiceUnavailable,
                        [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                }
            });

        });


        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
        });

    }
}
