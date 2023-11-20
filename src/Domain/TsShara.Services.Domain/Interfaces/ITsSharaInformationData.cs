using TsShara.Services.Domain.Enum;

namespace TsShara.Services.Domain.Interfaces;

public interface ITsSharaInformationData:ITsSharaInformationDataRaw
{
    decimal? Temperature { get;  }

    decimal? Frequency { get;  }

    decimal? Out { get;  }

    decimal? In { get;}
    DateTime? UpdateTime { get;}
    decimal? Battery { get;}
    PowerSource? PowerSource { get;}
    TimeSpan? EstimatedBatteryTime { get; }
    TimeSpan? EstimatedChargingTime { get; }
    bool IsCharging { get; }
    bool IsDischarging { get; }
    bool IsOnBattery { get; }
    bool IsPlugged { get; }
    bool IsFullyCharged { get; }
    bool IsLowBattery { get; }
    bool IsCriticalBattery { get; }
    string RawMessage { get; } 
    string NormalizedRawMessage { get;}


}