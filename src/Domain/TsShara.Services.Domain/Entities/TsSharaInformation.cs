using System.IO.Ports;
using TsShara.Services.Domain.Interfaces;

namespace TsShara.Services.Domain.Entities;

internal class TsSharaInformation:ITsSharaSerialPortInformation
{
    public TsSharaInformation(SerialPort serialPort,ITsSharaInformationDataRaw? rawData)
    {
        SerialPort = serialPort;
        Data = new TsSharaInformationData(rawData);
    }

    public TsSharaInformation(SerialPort serial, ITsSharaInformationData data)
    {
        SerialPort = serial;
        Data = data;
    }
    public TsSharaInformation(SerialPort serialPort, Exception ex)
    {
        SerialPort = serialPort;
        Data = new TsSharaInformationError(serialPort,ex);
    }

    public ITsSharaInformation? Data { get; }

    public string PortName => SerialPort.PortName;
 
    public SerialPort SerialPort { get; }
}