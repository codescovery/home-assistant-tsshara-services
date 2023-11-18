using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TsShara.Services.Domain.Extensions.DependencyInjection;

public static class CommandLineExtensions
{
    public static IConfigurationBuilder AddCommandLine<T>(this IConfigurationBuilder builder, string[] args,
        IDictionary<string, object>? switchMaps = null)
    where T:class
    {
        builder.AddCommandLine(args, switchMaps);
        return builder;
    }
}