using System.Reflection;
using Codescovery.Library.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.OpenApi.Models;
using TsShara.Services.Domain.Configurations;
using TsShara.Services.Domain.Extensions.DependencyInjection;
using TsShara.Services.Domain.Interfaces;
const string AllowAllCorsPolicyName = "AllowAllPolicy";
var builder = WebApplication
    .CreateBuilder(args);
builder.WebHost
    .ConfigureAppConfiguration((context, configurationBuilder) =>
        {
            var assemblyLocation = Assembly.GetEntryAssembly()?.Location;
            var currentPath = Path.GetDirectoryName(assemblyLocation);
            if (string.IsNullOrWhiteSpace(currentPath)) throw new ArgumentNullException(nameof(currentPath));
            configurationBuilder.SetBasePath(currentPath!)
                .AddJsonFile("appsettings.json", false, true)
                .AddCommandLine(args)
                .AddEnvironmentVariables();
        }
    ).ConfigureServices((context, services) =>
    {
        services.AddEnabledFeaturesService(args);
        var enabledFeaturesService = services.BuildServiceProvider().GetRequiredService<IEnabledFeaturesService>();
        services.AddAppSettings<AppSettings>(context.Configuration);
        services.AddDefaultAvailableSerialUsbPorts();
        services.AddDefaultTsSharaStatusFromUsb();
        services.AddDefaultTsSharaInformationDataService();
        services.AddTimeSpanServices(ServiceLifetime.Singleton);
        services.AddFeatures(args);
        if (!enabledFeaturesService.IsFeatureApiEnabled) return;
        services.AddCors(c => c
            .AddPolicy(AllowAllCorsPolicyName, p =>
                p.SetIsOriginAllowed(s => true)
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                ));
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            var url = $"{context.Configuration["Api:Endpoint"]}";
            if (string.IsNullOrWhiteSpace(url)) return;
            c.SwaggerGeneratorOptions.Servers = new List<OpenApiServer>
            {
                new()
                {
                    Url = url,
                    Description = "Api"
                }
            };
        });
    });
var app = builder.Build();
var enabledFeaturesService = app.Services.GetRequiredService<IEnabledFeaturesService>();
app.UseCors(AllowAllCorsPolicyName);
if (enabledFeaturesService.IsFeatureApiEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapControllers();
}

app.Run();