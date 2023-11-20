using Codescovery.Library.Commons.Interfaces.TimeSpan;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Runtime;
using TsShara.Services.Domain.Configurations;
using TsShara.Services.Domain.Enum;
using TsShara.Services.Domain.Interfaces;

namespace TsShara.Services.Domain.Services;

internal class ConsoleMonitoringService : BackgroundService, IBackgroundServiceStatus
{
    private readonly ILogger<ConsoleMonitoringService> _logger;
    private readonly ITsSharaStatusFromUsb _serialUsbReader;
    private readonly IOptions<AppSettings> _settings;
    private readonly ITimeSpanService _timeSpanService;
    private readonly IEnabledFeaturesService _enabledFeaturesService;
    public BackgroundServiceStatus Status { get; private set; }
    public ConsoleMonitoringService(ILogger<ConsoleMonitoringService> logger, IOptions<AppSettings> settings, ITsSharaStatusFromUsb serialUsbReader, ITimeSpanService timeSpanService, IEnabledFeaturesService enabledFeaturesService)
    {
        _logger = logger;
        _settings = settings;
        _serialUsbReader = serialUsbReader;
        _timeSpanService = timeSpanService;
        _enabledFeaturesService = enabledFeaturesService;
        Status = BackgroundServiceStatus.Created;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        Status = BackgroundServiceStatus.Stopped;
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Console Monitoring running at: {time}", DateTimeOffset.Now);
        Status = BackgroundServiceStatus.Running;
        var delay = _timeSpanService.ToTimeSpan(_settings.Value.Notifier.Interval);
        while (!stoppingToken.IsCancellationRequested && _enabledFeaturesService.IsFeatureConsoleMonitoringEnabled)
        {
            try
            {
                var data = string.IsNullOrWhiteSpace(_settings.Value.TsShara.SerialPortName)
                    ?await _serialUsbReader.GetAsync(stoppingToken)
                    :await _serialUsbReader.GetAsync(_settings.Value.TsShara.SerialPortName, stoppingToken);
                await HandleDataAsync(data, stoppingToken);
                await Task.Delay(delay, stoppingToken);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Unhandled error");
            }
        }
    }

    private async Task HandleDataAsync(ITsSharaInformationResult? data, CancellationToken cancellationToken)
    {
        if (data == null)
            return;

        if (data.IsType<ITsSharaInformationError>())
            await HandleErrorAsync(data.AsError()!, cancellationToken);
        else if (data.IsType<ITsSharaInformationData>())
            await HandleDataAsync(data.AsData()!, cancellationToken);
        else if (data.IsType<ITsSharaInformationDataRaw>())
            await HandleDataRawAsync(data.AsDataRaw()!, cancellationToken);

    }

    private async Task HandleDataRawAsync(ITsSharaInformationDataRaw dataRaw, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Raw data: {rawData}", dataRaw.RawData);
        await Task.CompletedTask;
    }

    private async Task HandleDataAsync(ITsSharaInformationData? data, CancellationToken cancellationToken)
    {
        if (data == null) return;

        Console.Clear();

        if (data is { IsPlugged: true, IsFullyCharged: true })
            _logger.LogInformation("In: {in}v  Out: {out}v  Frequency: {frequency}hz  Temperature: {temperature}º  PowerSource: {powerSource}  Battery: {battery}%  UpdateTime: {updateTime}", data.In, data.Out, data.Frequency, data.Temperature, data.PowerSource, data.Battery, data.UpdateTime);
        if (data is { IsPlugged: true, IsFullyCharged: false })
            _logger.LogInformation("In: {in}v  Out: {out}v  Frequency: {frequency}hz  Temperature: {temperature}º PowerSource: {powerSource}  Battery: {battery}%  UpdateTime: {updateTime}  EstimatedChargingTime: {estimatedChargingTime:mm} minutes  ", data.In, data.Out, data.Frequency, data.Temperature, data.PowerSource, data.Battery, data.UpdateTime, data.EstimatedChargingTime);
        if (data is { IsPlugged: false, IsDischarging: true })
            _logger.LogInformation("In: {in}v  Out: {out}v  Frequency: {frequency}hz Temperature: {temperature}º PowerSource: {powerSource}  Battery: {battery}%  UpdateTime: {updateTime}  EstimatedBatteryTime: {estimatedBatteryTime:mm} minutes  ", data.In, data.Out, data.Frequency, data.Temperature, data.PowerSource, data.Battery, data.UpdateTime, data.EstimatedBatteryTime);
        
        _logger.LogInformation("Raw data: {rawData}", data.RawMessage);
        _logger.LogInformation("Normalized raw data: {normalizedRawData}", data.NormalizedRawMessage);
        await Task.CompletedTask;
    }

    private async Task HandleErrorAsync(ITsSharaInformationError error, CancellationToken cancellationToken)
    {
        _logger.LogError(error.Exception, "Error on port {port}", error.PortName);
        await Task.CompletedTask;
    }
}