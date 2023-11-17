using TsShara.Services.Domain.Enum;
using TsShara.Services.Domain.Interfaces;

namespace TsShara.Services.Domain.Entities;

internal class TsSharaInformationData(ITsSharaInformationDataRaw rawData) : ITsSharaInformationData
{
    public string DeviceInfo { get; } = rawData.DeviceInfo;
    public string RawData { get; } = rawData.RawData;
    public string PortName { get; } = rawData.PortName;
    public decimal? Temperature { get; init; }

    public decimal? Frequency { get; init; }

    public decimal? Out { get; init; }

    public decimal? In { get; init; }
    public DateTime? UpdateTime { get; init; }
    public decimal? Battery { get; init; }
    public PowerSource? PowerSource { get; init; }
    public TimeSpan? EstimatedBatteryTime => CalculateEstimatedBatteryTime();
    public TimeSpan? EstimatedChargingTime => CalculateEstimatedChargingTime();

    public bool IsCharging => PowerSource == Enum.PowerSource.Network;
    public bool IsDischarging => PowerSource == Enum.PowerSource.Battery;
    public bool IsOnBattery => PowerSource == Enum.PowerSource.Battery;
    public bool IsPlugged => PowerSource == Enum.PowerSource.Network;
    public bool IsFullyCharged => Battery == 100;
    public bool IsLowBattery => Battery <= 25;
    public bool IsCriticalBattery => Battery <= 15;
    private TimeSpan? CalculateEstimatedBatteryTime()
    {
        if (PowerSource != Enum.PowerSource.Battery || !Out.HasValue || !In.HasValue)
            return TimeSpan.Zero;
        var netPowerPerMillisecond = (In.Value - Out.Value) / 3600000;
        if (netPowerPerMillisecond >= 0) return TimeSpan.Zero;
        var millisecondsToEmptyBattery = Battery.GetValueOrDefault() / (-netPowerPerMillisecond / 100);
        return TimeSpan.FromMilliseconds((double)millisecondsToEmptyBattery);

    }
    private TimeSpan CalculateEstimatedChargingTime()
    {
        if (PowerSource != Enum.PowerSource.Network || !Out.HasValue || !In.HasValue)
            return TimeSpan.Zero;
        var netPowerPerMillisecond = (In.Value - Out.Value) / 3600000;
        if (netPowerPerMillisecond <= 0) return TimeSpan.Zero;
        var remainingBatteryPercentage = 100 - Battery.GetValueOrDefault();
        var millisecondsToFullCharge = remainingBatteryPercentage / (netPowerPerMillisecond / 100);
        return TimeSpan.FromMilliseconds((double)millisecondsToFullCharge);

    }
}