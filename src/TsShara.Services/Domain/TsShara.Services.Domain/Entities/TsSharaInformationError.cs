using System.IO.Ports;
using TsShara.Services.Domain.Interfaces;

namespace TsShara.Services.Domain.Entities;

internal class TsSharaInformationError:ITsSharaInformationError
{
    public string PortName { get; }
    public Exception Exception { get; }

    public TsSharaInformationError(SerialPort serialPort, Exception exception)
    {
        PortName = serialPort.PortName;
        Exception = exception;
    }

    public TsSharaInformationError(string portName, Exception exception)
    {
        PortName = portName;
        Exception = exception;
    }


}