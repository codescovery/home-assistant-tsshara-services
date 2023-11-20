using Codescovery.Library.Commons.Entities.Configurations;

namespace TsShara.Services.Domain.Configurations;

public class ConsoleMonitoring
{
    public TimeSpanConfiguration Interval { get; set; } = null!;
}