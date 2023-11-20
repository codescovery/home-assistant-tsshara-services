using System.Text.Json.Serialization;
using TsShara.Services.Domain.Interfaces;

namespace TsShara.Services.Application.ViewModels;

public class TsSharaStatusRawResponseModel:TsSharaStatusResponseModel,ITsSharaInformationDataRaw
{
    public TsSharaStatusRawResponseModel(ITsSharaInformationDataRaw raw, bool isMonitoring, string portName) : base(isMonitoring, portName)
    {
        DeviceInfo = raw.DeviceInfo;
        RawData = raw.RawData;
    }

    [JsonPropertyName("deviceInfo")]
    public string DeviceInfo { get; }
    [JsonPropertyName("rawData")]
    public string RawData { get; }
}