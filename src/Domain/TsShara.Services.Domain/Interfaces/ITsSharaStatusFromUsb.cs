using System.IO.Ports;
using TsShara.Services.Domain.Constants;

namespace TsShara.Services.Domain.Interfaces;

public interface ITsSharaStatusFromUsb:IDisposable
{
    bool IsMonitoring { get; }
    ITsSharaSerialPortInformation? MonitoredPort { get; }
    SerialPort CreateSerialPort(string name, int baudRate = DefaultValues.BaudRate, int dataBits = DefaultValues.DataBits, Parity parity = Parity.None, StopBits stopBits = StopBits.One, Handshake handshake = Handshake.None);
    void OpenConnectionIfClosed();
    void OpenConnectionIfClosed(SerialPort port);
    void CloseConnectionIfOpen();
    Task<ITsSharaInformationResult?> GetAsync(CancellationToken cancellationToken);
    Task<ITsSharaInformationResult?> GetAsync(string portName, CancellationToken cancellationToken);
    Task<ITsSharaInformationResult?> ReadDataAsync(CancellationToken cancellationToken);
    void StopMonitoring();
}