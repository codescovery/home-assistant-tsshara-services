using System.Text.Json.Serialization;
using TsShara.Services.Domain.Enum;
using TsShara.Services.Domain.Interfaces;

namespace TsShara.Services.Application.ViewModels;

public class TsSharaStatusDataResponseModel:TsSharaStatusResponseModel,ITsSharaInformationData
{
    public TsSharaStatusDataResponseModel(ITsSharaInformationData data, bool isMonitoring, string portName) : base(isMonitoring, portName)
    {
        DeviceInfo = data.DeviceInfo;
        RawData = data.RawData;
        Temperature = data.Temperature;
        Frequency = data.Frequency;
        Out = data.Out;
        In = data.In;
        UpdateTime = data.UpdateTime;
        Battery = data.Battery;
        PowerSource = data.PowerSource;
        EstimatedBatteryTime = data.EstimatedBatteryTime;
        EstimatedChargingTime = data.EstimatedChargingTime;
        IsCharging = data.IsCharging;
        IsDischarging = data.IsDischarging;
        IsOnBattery = data.IsOnBattery;
        IsPlugged = data.IsPlugged;
        IsFullyCharged = data.IsFullyCharged;
        IsLowBattery = data.IsLowBattery;
        IsCriticalBattery = data.IsCriticalBattery;
        RawMessage = data.RawMessage;
        NormalizedRawMessage = data.NormalizedRawMessage;
        
        
    }

    [JsonPropertyName("deviceInfo")]
    public string DeviceInfo { get; }
    [JsonPropertyName("rawData")]
    public string RawData { get; }
    [JsonPropertyName("temperature")]
    public decimal? Temperature { get; }
    [JsonPropertyName("frequency")]
    public decimal? Frequency { get; }
    [JsonPropertyName("out")]
    public decimal? Out { get; }
    [JsonPropertyName("in")]
    public decimal? In { get; }
    [JsonPropertyName("updateTime")]
    public DateTime? UpdateTime { get; }
    [JsonPropertyName("battery")]
    public decimal? Battery { get; }
    [JsonPropertyName("powerSource")]
    public PowerSource? PowerSource { get; }
    [JsonPropertyName("estimatedBatteryTime")]
    public TimeSpan? EstimatedBatteryTime { get; }
    [JsonPropertyName("estimatedChargingTime")]
    public TimeSpan? EstimatedChargingTime { get; }
    [JsonPropertyName("isCharging")]
    public bool IsCharging { get; }
    [JsonPropertyName("isDischarging")]
    public bool IsDischarging { get; }
    [JsonPropertyName("isOnBattery")]
    public bool IsOnBattery { get; }
    [JsonPropertyName("isPlugged")]
    public bool IsPlugged { get; }
    [JsonPropertyName("isFullyCharged")]
    public bool IsFullyCharged { get; }
    [JsonPropertyName("isLowBattery")]
    public bool IsLowBattery { get; }
    [JsonPropertyName("isCriticalBattery")]
    public bool IsCriticalBattery { get; }
    [JsonPropertyName("rawMessage")]
    public string RawMessage { get; }
    [JsonPropertyName("normalizedRawMessage")]
    public string NormalizedRawMessage { get; }
}