using TsShara.Services.Domain.Entities;

namespace TsShara.Services.Domain.Interfaces;

public interface IAvailableSerialUsbPorts
{
    IEnumerable<SerialPortInfo> Get();
}