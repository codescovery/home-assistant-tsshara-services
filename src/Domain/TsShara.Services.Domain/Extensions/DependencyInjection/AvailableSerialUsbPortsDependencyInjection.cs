using Codescovery.Library.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using TsShara.Services.Domain.Interfaces;
using TsShara.Services.Domain.Services;

namespace TsShara.Services.Domain.Extensions.DependencyInjection;

public static class AvailableSerialUsbPortsDependencyInjection
{
    public static IServiceCollection AddDefaultAvailableSerialUsbPorts(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
    {
        return services.AddAvailableSerialUsbPorts<AvailableSerialUsbPorts>(serviceLifetime);
    }

    public static IServiceCollection AddAvailableSerialUsbPorts<T>(this IServiceCollection services
        , ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) where T : class, IAvailableSerialUsbPorts
    {
        services.Add<IAvailableSerialUsbPorts, T>(serviceLifetime);
        return services;
    }
}