using System.Text.Json.Serialization;

namespace TsShara.Services.Application.ViewModels;

public class TsSharaStatusResponseModel
{
    

    public TsSharaStatusResponseModel(bool isMonitoring, string? portName)
    {
        IsMonitoring = isMonitoring;
        PortName = portName;
    }
    [JsonPropertyName("isMonitoring")]
    public bool IsMonitoring { get; }
    [JsonPropertyName("portName")]
    public string? PortName { get; }
}