using Codescovery.Library.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using TsShara.Services.Domain.Interfaces;
using TsShara.Services.Domain.Services;

namespace TsShara.Services.Domain.Extensions.DependencyInjection;

public static class TsSharaInformationDataServiceDependencyInjection
{
    public static IServiceCollection AddDefaultTsSharaInformationDataService(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
    {
        return services.AddTsSharaInformationDataService<TsSharaInformationDataService>(serviceLifetime);
    }

    public static IServiceCollection AddTsSharaInformationDataService<T>(this IServiceCollection services
        , ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) where T : class, ITsSharaInformationDataService
    {
        services.Add<ITsSharaInformationDataService, T>(serviceLifetime);
        return services;
    }

}