namespace TsShara.Services.Domain.Interfaces;

public interface ITsSharaInformationResult
{
    ITsSharaInformation? Data { get; }
    ITsSharaInformationError? AsError();
    ITsSharaInformationDataRaw? AsDataRaw();
    ITsSharaInformationData? AsData();
    bool IsType<T>();
}