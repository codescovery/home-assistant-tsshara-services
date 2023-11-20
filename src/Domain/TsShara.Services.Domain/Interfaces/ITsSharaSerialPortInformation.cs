using System.IO.Ports;

namespace TsShara.Services.Domain.Interfaces;

public interface ITsSharaSerialPortInformation:ITsSharaInformation
{
    SerialPort SerialPort { get; }
}