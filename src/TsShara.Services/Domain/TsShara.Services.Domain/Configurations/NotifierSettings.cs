using Codescovery.Library.Commons.Entities.Configurations;

namespace TsShara.Services.Domain.Configurations;

public class NotifierSettings
{
    public bool Enabled { get; set; }
    public TimeSpanConfiguration Interval { get; set; } = null!;
    public string? Endpoint { get; set; }
}