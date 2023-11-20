using Codescovery.Library.DependencyInjection.Abstractions;
using TsShara.Services.Domain.Configurations.TsShara;

namespace TsShara.Services.Domain.Configurations;

public class AppSettings : BaseAppSettings
{
    public TsSharaSettings TsShara { get; set; } = null!;
    public NotifierSettings Notifier { get; set; } = null!;
    public ConsoleMonitoring ConsoleMonitoring { get; set; } = null!;
}