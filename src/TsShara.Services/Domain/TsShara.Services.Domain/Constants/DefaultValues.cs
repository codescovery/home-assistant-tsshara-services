using System.IO.Ports;

namespace TsShara.Services.Domain.Constants;

public class DefaultValues
{
    public const int BaudRate = 2400;
    public const int DataBits = 8;
    public const Parity Parity = System.IO.Ports.Parity.None;
    public const StopBits StopBits = System.IO.Ports.StopBits.One;
    public const Handshake Handshake = System.IO.Ports.Handshake.None;

}