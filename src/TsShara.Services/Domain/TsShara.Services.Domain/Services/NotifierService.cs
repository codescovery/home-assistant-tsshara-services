using System.Text;
using System.Text.Json;
using Codescovery.Library.Commons.Interfaces.TimeSpan;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TsShara.Services.Domain.Configurations;
using TsShara.Services.Domain.Enum;
using TsShara.Services.Domain.Interfaces;

namespace TsShara.Services.Domain.Services
{
    public class NotifierService : BackgroundService, IBackgroundServiceStatus
    {
        private readonly ILogger<NotifierService> _logger;
        private readonly ITsSharaStatusFromUsb _serialUsbReader;
        private readonly IOptions<AppSettings> _settings;
        private readonly HttpClient _httpClient;
        private readonly ITimeSpanService _timeSpanService;
        public BackgroundServiceStatus Status { get; private set; }
        public NotifierService(ILogger<NotifierService> logger, IOptions<AppSettings> settings, ITsSharaStatusFromUsb serialUsbReader, HttpClient httpClient, ITimeSpanService timeSpanService)
        {
            _logger = logger;
            _settings = settings;
            _serialUsbReader = serialUsbReader;
            _httpClient = httpClient;
            _timeSpanService = timeSpanService;
            Status = BackgroundServiceStatus.Created;
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Status = BackgroundServiceStatus.Stopped;
            return base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("NotifierService running at: {time}", DateTimeOffset.Now);
            Status = BackgroundServiceStatus.Running;
            var delay = _timeSpanService.ToTimeSpan(_settings.Value.Notifier.Interval);
            while (!stoppingToken.IsCancellationRequested && _settings.Value.Notifier.Enabled)
            {
                try
                {
                    var data = await _serialUsbReader.GetAsync(stoppingToken);
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
            {
                _logger.LogInformation("No data");
                return;
            }
            
            if (!_settings.Value.Notifier.Enabled) return;
                
            if(data.IsType<ITsSharaInformationError>())
                await NotifyAsync(data.AsError()!, cancellationToken);
            else if(data.IsType<ITsSharaInformationData>())
                await NotifyAsync(data.AsData()!, cancellationToken);
            else if(data.IsType<ITsSharaInformationDataRaw>())
                await NotifyAsync(data.AsDataRaw()!, cancellationToken);

        }

        private async Task NotifyAsync<T>(T data, CancellationToken cancellationToken)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response =
                    await _httpClient.PostAsync(_settings.Value.Notifier.Endpoint, content, cancellationToken);
                response.EnsureSuccessStatusCode();

            }
            catch (Exception e)
            {
               _logger.LogError(e, $"Error notifying to endpoint:{_settings.Value.Notifier.Endpoint}");
            }
        }
    }
}