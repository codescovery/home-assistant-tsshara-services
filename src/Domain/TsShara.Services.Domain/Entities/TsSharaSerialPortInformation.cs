using System.IO.Ports;
using TsShara.Services.Domain.Interfaces;

namespace TsShara.Services.Domain.Entities;

internal class TsSharaSerialPortInformation:TsSharaInformation,ITsSharaSerialPortInformation
{
    public TsSharaSerialPortInformation(SerialPort serialPort, ITsSharaInformationDataRaw? rawData) : base(serialPort, rawData)
    {
    }

    public TsSharaSerialPortInformation(SerialPort serialPort, Exception ex) : base(serialPort, ex)
    {
    }
}