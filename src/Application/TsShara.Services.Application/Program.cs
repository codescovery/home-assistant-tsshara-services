using System.Reflection;
using Codescovery.Library.DependencyInjection.Extensions;
using TsShara.Services.Domain.Configurations;
using TsShara.Services.Domain.Extensions.DependencyInjection;
using TsShara.Services.Domain.Interfaces;

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
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
});
var app = builder.Build();
var enabledFeaturesService = app.Services.GetRequiredService<IEnabledFeaturesService>();
if (enabledFeaturesService.IsFeatureApiEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapControllers();
}

app.Run();