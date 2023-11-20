using System.IO.Ports;
using System.Management;
using System.Runtime.InteropServices;
using TsShara.Services.Domain.Entities;
using TsShara.Services.Domain.Interfaces;

namespace TsShara.Services.Domain.Services;

internal class AvailableSerialUsbPorts: IAvailableSerialUsbPorts
{
    public IEnumerable<SerialPortInfo> Get()
    {
        var serialPortInfos = new List<SerialPortInfo>();
        foreach (var portName in SerialPort.GetPortNames())
        {
            var portId = GetSerialPortId(portName);
            serialPortInfos.Add(new (portName,portId));
        }
        return serialPortInfos;
    }
    static string? GetSerialPortId(string portName)
    {
        return null;
    }
}