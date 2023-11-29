using System.Text.Json.Serialization;
using TsShara.Services.Domain.Interfaces;

namespace TsShara.Services.Application.ViewModels;

public class TsSharaStatusErrorResponseModel:TsSharaStatusResponseModel
{
    [JsonPropertyName("error")]
    public ITsSharaInformationError? Error { get; }
    
    public TsSharaStatusErrorResponseModel( bool isMonitoring, ITsSharaInformationError? error=null) : base(isMonitoring, error?.PortName)
    {
        Error = error;
    }
    public TsSharaStatusErrorResponseModel(ITsSharaInformationError? error) : this(false, error)
    {
    }
}