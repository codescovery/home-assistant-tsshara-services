namespace TsShara.Services.Domain.Entities;

public class SerialPortInfo(string portName, string? portId)
{
    public string PortName { get; } = portName;
    public string? PortId { get; } = portId;
}