namespace TsShara.Services.Domain.Interfaces;

public interface ITsSharaInformationDataRaw:ITsSharaInformation
{
    string DeviceInfo { get; }
    string RawData { get; }
}