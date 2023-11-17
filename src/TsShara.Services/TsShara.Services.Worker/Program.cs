using Codescovery.Library.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using TsShara.Services.Domain.Configurations;
using TsShara.Services.Domain.Extensions.DependencyInjection;
using TsShara.Services.Domain.Services;
using TsShara.Services.Worker;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context,services) =>
    {
        services.AddAppSettings<AppSettings>(context.Configuration);
        
        services.AddDefaultAvailableSerialUsbPorts();
        services.AddDefaultTsSharaStatusFromUsb();
        services.AddDefaultTsSharaInformationDataService();
        services.AddTimeSpanServices(ServiceLifetime.Singleton);
        services.AddHostedService<Worker>();
    })
    .Build();
host.Run();
