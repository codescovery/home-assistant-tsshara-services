using System.IO.Ports;

namespace TsShara.Services.Domain.Interfaces;

public interface ITsSharaInformationDataService
{
    ITsSharaInformation? Create(SerialPort serialPort, ITsSharaInformationDataRaw dataRaw);
    ITsSharaInformation? Create(SerialPort serialPort, Exception ex);
    ITsSharaInformationDataRaw Create(string deviceInfo, SerialPort serialPort, string rawData);
    ITsSharaInformationDataRaw Create(SerialPort serialPort, string rawData);
}