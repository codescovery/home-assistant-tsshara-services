using Codescovery.Library.Commons.Entities.Configurations;

namespace TsShara.Services.Domain.Configurations.TsShara;

public class TsSharaCommandSettings
{
    public string Command { get; set; } = null!;
    public TimeSpanConfiguration Delay { get; set; } = null!;
}