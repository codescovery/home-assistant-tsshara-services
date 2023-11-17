using Codescovery.Library.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using TsShara.Services.Domain.Interfaces;
using TsShara.Services.Domain.Services;

namespace TsShara.Services.Domain.Extensions.DependencyInjection;

public static class TsSharaStatusFromUsbDependencyInjection
{
    public static IServiceCollection AddDefaultTsSharaStatusFromUsb(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
    {
        return services.AddTsSharaStatusFromUsb<TsSharaStatusFromUsb>(serviceLifetime);
    }

    public static IServiceCollection AddTsSharaStatusFromUsb<T>(this IServiceCollection services
        , ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) where T : class, ITsSharaStatusFromUsb
    {
        services.Add<ITsSharaStatusFromUsb, T>(serviceLifetime);
        return services;
    }
}