using ConsoleTables;
using Microsoft.Extensions.Options;
using TsShara.Services.Domain.Configurations;
using TsShara.Services.Domain.Interfaces;
using TsShara.Services.Domain.Services;

namespace TsShara.Services.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ITsSharaStatusFromUsb _serialUsbReader;

        public Worker(ILogger<Worker> logger, IOptions<AppSettings> settings, ITsSharaStatusFromUsb serialUsbReader)
        {
            _logger = logger;
            _serialUsbReader = serialUsbReader;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var data = await _serialUsbReader.GetAsync(stoppingToken);
                    await HandleDataAsync(data, stoppingToken);
                    await Task.Delay(1000, stoppingToken);
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
            {
                _logger.LogInformation("No data");
                return;
            }
            
            if(data.IsType<ITsSharaInformationError>())
                await HandleErrorAsync(data.AsError()!, cancellationToken);
            else if (data.IsType<ITsSharaInformationData>())
                await HandleDataAsync(data.AsData()!, cancellationToken);
            else if(data.IsType<ITsSharaInformationDataRaw>())
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

            if(data is { IsPlugged: true, IsFullyCharged: true })
                _logger.LogInformation("In: {in}v  Out: {out}v  Frequency: {frequency}hz  Temperature: {temperature}º  PowerSource: {powerSource}  Battery: {battery}%  UpdateTime: {updateTime}", data.In, data.Out, data.Frequency, data.Temperature, data.PowerSource, data.Battery, data.UpdateTime);
            if(data is {IsPlugged:true, IsFullyCharged:false})
                _logger.LogInformation("In: {in}v  Out: {out}v  Frequency: {frequency}hz  Temperature: {temperature}º PowerSource: {powerSource}  Battery: {battery}%  UpdateTime: {updateTime}  EstimatedChargingTime: {estimatedChargingTime:mm} minutes  ", data.In, data.Out, data.Frequency, data.Temperature, data.PowerSource, data.Battery, data.UpdateTime, data.EstimatedChargingTime);
            if(data is {IsPlugged:false, IsDischarging:true})
                _logger.LogInformation("In: {in}v  Out: {out}v  Frequency: {frequency}hz Temperature: {temperature}º PowerSource: {powerSource}  Battery: {battery}%  UpdateTime: {updateTime}  EstimatedBatteryTime: {estimatedBatteryTime:mm} minutes  ", data.In, data.Out, data.Frequency, data.Temperature, data.PowerSource, data.Battery, data.UpdateTime, data.EstimatedBatteryTime);

            await Task.CompletedTask;
        }

        private async Task HandleErrorAsync(ITsSharaInformationError error, CancellationToken cancellationToken)
        {
            _logger.LogError(error.Exception, "Error on port {port}", error.PortName);
            await Task.CompletedTask;
        }

        public override void Dispose()
        {
            _serialUsbReader.Dispose();
            base.Dispose();
        }
    }
}