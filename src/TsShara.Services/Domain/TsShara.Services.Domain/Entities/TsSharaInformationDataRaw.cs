using TsShara.Services.Domain.Interfaces;

namespace TsShara.Services.Domain.Entities;

internal class TsSharaInformationDataRaw
    (string portName, string deviceInfo, string rawData) : ITsSharaInformationDataRaw
{
    public string PortName { get; } = portName;
    public string DeviceInfo { get; } = deviceInfo;
    public string RawData { get; } = rawData;
}