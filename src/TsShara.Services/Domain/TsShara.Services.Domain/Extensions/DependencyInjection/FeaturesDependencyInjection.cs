using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.PortableExecutable;
using Microsoft.Extensions.Options;
using TsShara.Services.Domain.Configurations;
using TsShara.Services.Domain.Interfaces;
using TsShara.Services.Domain.Services;
using Codescovery.Library.Commons.Extensions;

namespace TsShara.Services.Domain.Extensions.DependencyInjection;

public static class FeaturesDependencyInjection
{
    public static IServiceCollection AddConsoleMonitoring(this IServiceCollection services)
    {
        services.AddFeaturesService();
        //check if the service is already registered
        var serviceDescriptor = services.FirstOrDefault(x => x.ServiceType == typeof(ConsoleMonitoringService));
        if (serviceDescriptor != null) return services;
        services.AddHostedService<ConsoleMonitoringService>();
        services.AddSingleton<ConsoleMonitoringService>();
        return services;
    }

    public static IServiceCollection AddNotifier(this IServiceCollection services)
    {
        services.AddFeaturesService();
        //check if the service is already registered
        var serviceDescriptor = services.FirstOrDefault(x => x.ServiceType == typeof(NotifierService));
        if (serviceDescriptor != null) return services;
        var settings = services.BuildServiceProvider().GetRequiredService<IOptions<AppSettings>>().Value;
        if (string.IsNullOrWhiteSpace(settings.Notifier.Endpoint)) return services;
        services.AddHttpClient<NotifierService>(client =>
        {

            client.BaseAddress = new Uri(settings.Notifier.Endpoint);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
        services.AddHostedService<NotifierService>();
        services.AddSingleton<NotifierService>();
        return services;
    }

    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {

        services.AddConsoleMonitoring();
        services.AddNotifier();
        return services;
    }

    public static IServiceCollection AddFeaturesService(this IServiceCollection services)
    {
        //check if the service is already registered
        var serviceDescriptor = services.FirstOrDefault(x => x.ServiceType == typeof(IFeatureService));
        if (serviceDescriptor != null) return services;
        services.AddSingleton<IFeatureService, FeatureService>();
        return services;
    }
}