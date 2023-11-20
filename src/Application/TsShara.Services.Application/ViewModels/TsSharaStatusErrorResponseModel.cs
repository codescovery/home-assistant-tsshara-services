using System.Text.Json.Serialization;
using TsShara.Services.Domain.Interfaces;

namespace TsShara.Services.Application.ViewModels;

public class TsSharaStatusErrorResponseModel:TsSharaStatusResponseModel
{
    [JsonPropertyName("error")]
    public ITsSharaInformationError Error { get; }

    public TsSharaStatusErrorResponseModel(ITsSharaInformationError error, bool isMonitoring, string portName) : base(isMonitoring, portName)
    {
        Error = error;
    }
}