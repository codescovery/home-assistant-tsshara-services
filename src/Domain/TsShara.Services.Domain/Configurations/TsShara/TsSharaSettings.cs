using Codescovery.Library.Commons.Entities.Configurations;

namespace TsShara.Services.Domain.Configurations.TsShara;

public class TsSharaSettings
{
    public string? SerialPortName { get; set; }
    public TimeSpanConfiguration ReadTimeout { get; set; } = null!;
    public List<string> ValidPrefixes { get; set; } = null!;
    public TsSharaCommandsSettings Commands { get; set; } = null!;
    public string NewLineIndicator { get; set; } = null!;
}