using System.IO.Ports;
using System.Xml.Linq;
using Codescovery.Library.Commons.Interfaces.TimeSpan;
using Microsoft.Extensions.Logging;
using TsShara.Services.Domain.Configurations;
using TsShara.Services.Domain.Constants;
using TsShara.Services.Domain.Entities;
using TsShara.Services.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace TsShara.Services.Domain.Services;

public class TsSharaStatusFromUsb : ITsSharaStatusFromUsb
{
    private readonly IOptions<AppSettings> _settings;
    private readonly ILogger<TsSharaStatusFromUsb> _logger;
    private readonly IAvailableSerialUsbPorts _avaliableSerialUsbPorts;
    private readonly ITsSharaInformationDataService _tsSharaInformationDataService;
    private readonly ITimeSpanService _timeSpanService;
    public TsSharaStatusFromUsb(IOptions<AppSettings> settings,
        ILogger<TsSharaStatusFromUsb> logger,
        IAvailableSerialUsbPorts avaliableSerialUsbPorts,
        ITsSharaInformationDataService tsSharaInformationDataService,
        ITimeSpanService timeSpanService)
    {
        _settings = settings;
        _logger = logger;
        _avaliableSerialUsbPorts = avaliableSerialUsbPorts;
        _tsSharaInformationDataService = tsSharaInformationDataService;
        _timeSpanService = timeSpanService;
    }

    public bool IsMonitoring => MonitoredPort is { SerialPort.IsOpen: true };
    public ITsSharaSerialPortInformation? MonitoredPort { get; private set; }

    public SerialPort CreateSerialPort(string name, int baudRate = DefaultValues.BaudRate,
        int dataBits = DefaultValues.DataBits, Parity parity = Parity.None, StopBits stopBits = StopBits.One,
        Handshake handshake = Handshake.None)
    {
        if (MonitoredPort != null) return MonitoredPort.SerialPort;
        var serialPort = new SerialPort(name, baudRate, parity, dataBits, stopBits)
        {
            Handshake = handshake,
            NewLine = _settings.Value.TsShara.NewLineIndicator,
            ReadTimeout = _timeSpanService.ToTimeSpan(_settings.Value.TsShara.ReadTimeout).Milliseconds,
        };
        return serialPort;
    }

    public void OpenConnectionIfClosed()
    {
        if (MonitoredPort == null) return;
        if (MonitoredPort.SerialPort.IsOpen) return;
        MonitoredPort.SerialPort.Open();
    }

    public void OpenConnectionIfClosed(SerialPort port)
    {
        if (port.IsOpen) return;
        try
        {
            port.Open();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error opening port {name}", port.PortName);
        }
    }

    public void CloseConnectionIfOpen()
    {
        if (MonitoredPort == null) return;
        if (!MonitoredPort.SerialPort.IsOpen) return;
        MonitoredPort.SerialPort.Close();
        MonitoredPort.SerialPort.Dispose();
    }

    public async Task<ITsSharaInformationResult?> GetAsync(CancellationToken cancellationToken)
    {
        var availableSerialUsbPorts = _avaliableSerialUsbPorts.Get().ToList();
        if (!availableSerialUsbPorts.Any())
            return HandleNoConnectedUsbDevices();
        if (MonitoredPort != null)
            return await ReadDataAsync(cancellationToken);
        var found = await FindTsSharaPort(cancellationToken, availableSerialUsbPorts);
        if (found == null) return null;
        MonitoredPort = found;
        return await ReadDataAsync(cancellationToken);
    }

    public  async Task<ITsSharaInformationResult?> GetAsync(string portName, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(portName))
                return null;
            if(MonitoredPort!=null)
                return await ReadDataAsync(cancellationToken);
            if (MonitoredPort!=null && MonitoredPort.SerialPort.PortName != portName)
                StopMonitoring();
            var port = CreateSerialPort(portName);
            OpenConnectionIfClosed(port);
            var deviceInfo = await GetDeviceInfoAsync(port, cancellationToken);
            if (deviceInfo==null)
                return null;
            MonitoredPort =deviceInfo;
            return await ReadDataAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting data from port {name}", portName);
            return null;
        }
    }

    private ITsSharaInformationResult? HandleNoConnectedUsbDevices()
    {
        _logger.LogWarning("No serial usb ports found");
        return null;
    }

    private async Task<ITsSharaSerialPortInformation?> FindTsSharaPort(CancellationToken cancellationToken,
        List<SerialPortInfo> availableSerialUsbPorts)
    {
        _logger.LogDebug("Trying to find ts-shara compatible device info in {count} ports",
            availableSerialUsbPorts.Count);
        _logger.LogDebug("Ports: {name}", string.Join(",", availableSerialUsbPorts.Select(c => c.PortName)));
        foreach (var availableSerialUsbPort in availableSerialUsbPorts)
        {
            _logger.LogDebug("Trying to find ts-shara compatible device info in {name}",
                availableSerialUsbPort.PortName);
            var port = CreateSerialPort(availableSerialUsbPort.PortName);
            OpenConnectionIfClosed(port);
            _logger.LogDebug("Port {name} opened: {opened}", availableSerialUsbPort.PortName, port.IsOpen);
            var deviceInfo = await GetDeviceInfoAsync( port, cancellationToken);
            if (deviceInfo!=null)
                return deviceInfo;

            _logger.LogDebug("No device info found for {name}, closing connection", availableSerialUsbPort.PortName);
            port.Close();
            port.Dispose();
            _logger.LogDebug("Port {name} closed: {closed}", availableSerialUsbPort.PortName, !port.IsOpen);
        }

        _logger.LogWarning("No ts-shara compatible device info found in the following ports: {name}",
            string.Join(",", availableSerialUsbPorts.Select(c => c.PortName)));
        return null;
    }

    private async Task<TsSharaSerialPortInformation?> GetDeviceInfoAsync(SerialPort port, CancellationToken cancellationToken)
    {
        try
        {
            var deviceInfo = await ReadDeviceInfoAsync(port, cancellationToken);
            if (!IsTsharaDeviceInfoMessage(deviceInfo)) return null;
            _logger.LogDebug("Device info found for {name}", port.PortName);
            return new TsSharaSerialPortInformation(port, deviceInfo!);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting device info for {name}", port.PortName);
            return null;
        }

    }

    public async Task<ITsSharaInformationResult?> ReadDataAsync(CancellationToken cancellationToken)
    {
        if (MonitoredPort == null) return null;
        if (cancellationToken.IsCancellationRequested) return null;
        OpenConnectionIfClosed();
        MonitoredPort.SerialPort.Write(_settings.Value.TsShara.Commands.GetDeviceStatus.Command);
        var delay = _timeSpanService.ToTimeSpan(_settings.Value.TsShara.Commands.GetDeviceStatus.Delay);
        await Task.Delay(delay, cancellationToken);
        var data = MonitoredPort.SerialPort.ReadLine();
        var rawData = _tsSharaInformationDataService.Create(MonitoredPort.SerialPort, data);
        return new TsSharaInformationResult(_tsSharaInformationDataService.Create(MonitoredPort.SerialPort, rawData));
    }

    private async Task<ITsSharaInformationDataRaw?> ReadDeviceInfoAsync(SerialPort port,
        CancellationToken cancellationToken)

    {
        if (cancellationToken.IsCancellationRequested) return null;
        if (!port.IsOpen) return null;
        var delay = _timeSpanService.ToTimeSpan(_settings.Value.TsShara.Commands.GetDeviceInformation.Delay);
        try
        {
            port.DiscardInBuffer();
            port.DiscardOutBuffer();
            port.Write(_settings.Value.TsShara.Commands.GetDeviceInformation.Command);
            await Task.Delay(delay, cancellationToken);
            var deviceInfo = port.ReadExisting();
            if (string.IsNullOrWhiteSpace(deviceInfo)) return null;
            var rawData = _tsSharaInformationDataService.Create(deviceInfo, port, deviceInfo);
            return rawData;
        }
        catch (TimeoutException e)
        {
            _logger.LogError(e, "Timeout reading device info");
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading device info");
            throw;
        }
    }

    private bool IsTsharaDeviceInfoMessage(ITsSharaInformationDataRaw? data)
    {
        if (data == null) return false;
        if (string.IsNullOrWhiteSpace(data.RawData)) return false;
        if (_settings.Value.TsShara?.ValidPrefixes == null) return false;
        return _settings.Value.TsShara.ValidPrefixes.Any() &&
               _settings.Value.TsShara.ValidPrefixes.Any(data.RawData.StartsWith);
    }

    public void StopMonitoring()
    {
        if (MonitoredPort == null) return;
        CloseConnectionIfOpen();
    }


    public void Dispose()
    {
        if (MonitoredPort == null) return;
        CloseConnectionIfOpen();
        MonitoredPort.SerialPort.Dispose();
    }
}