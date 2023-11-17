using TsShara.Services.Domain.Enum;

namespace TsShara.Services.Domain.Interfaces;

public interface IBackgroundServiceStatus
{
    BackgroundServiceStatus Status { get;}
}