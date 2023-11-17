using TsShara.Services.Domain.Enum;
using TsShara.Services.Domain.Interfaces;

namespace TsShara.Services.Domain.Services;

internal class FeatureService: IFeatureService
{
    private readonly NotifierService? _notifierService;
    private readonly ConsoleMonitoringService? _consoleMonitoringService;

    public FeatureService(NotifierService? notifierService=null, ConsoleMonitoringService? consoleMonitoringService = null)
    {
        _notifierService = notifierService;
        _consoleMonitoringService = consoleMonitoringService;
    }

    public async Task StartNotifiersAsync(CancellationToken cancellationToken)
    {
        if(_notifierService is null) return;
        if(_notifierService.Status == BackgroundServiceStatus.Running) return;
        await _notifierService.StartAsync(cancellationToken);
    }

    public async Task StartConsoleMonitoringAsync(CancellationToken cancellationToken)
    {
        if(_consoleMonitoringService is null) return;
        if(_consoleMonitoringService.Status == BackgroundServiceStatus.Running) return;
        await _consoleMonitoringService.StartAsync(cancellationToken);
    }

    public async Task StopNotifiersAsync(CancellationToken cancellationToken)
    {
        if(_notifierService is null) return;
        if(_notifierService.Status == BackgroundServiceStatus.Stopped) return;
        await _notifierService.StopAsync(cancellationToken);
    }

    public async Task StopConsoleMonitoringAsync(CancellationToken cancellationToken)
    {
        if(_consoleMonitoringService is null) return;
        if(_consoleMonitoringService.Status == BackgroundServiceStatus.Stopped) return;
        await _consoleMonitoringService.StopAsync(cancellationToken);
    }
}