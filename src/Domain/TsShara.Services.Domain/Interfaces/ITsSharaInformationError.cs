namespace TsShara.Services.Domain.Interfaces;

public interface ITsSharaInformationError:ITsSharaInformation
{
    Exception Exception { get; }
}