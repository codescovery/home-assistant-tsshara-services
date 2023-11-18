using TsShara.Services.Domain.Enum;
using TsShara.Services.Domain.Interfaces;

namespace TsShara.Services.Domain.Services;

internal class FeatureService: IFeatureService
{
    private readonly NotifierService? _notifierService;
    private readonly ConsoleMonitoringService? _consoleMonitoringService;

    public FeatureService(IEnabledFeaturesService enabledFeaturesService, NotifierService? notifierService=null, ConsoleMonitoringService? consoleMonitoringService = null)
    {
        EnabledFeaturesService = enabledFeaturesService;
        _notifierService = notifierService;
        _consoleMonitoringService = consoleMonitoringService;
    }

    public IEnabledFeaturesService EnabledFeaturesService { get; }

    public async Task StartNotifiersAsync(CancellationToken cancellationToken)
    {
        if(!EnabledFeaturesService.IsFeatureNotifierEnabled) return;
        if(_notifierService is null) return;
        if(_notifierService.Status == BackgroundServiceStatus.Running) return;
        await _notifierService.StartAsync(cancellationToken);
    }

    public async Task StartConsoleMonitoringAsync(CancellationToken cancellationToken)
    {
        if(!EnabledFeaturesService.IsFeatureConsoleMonitoringEnabled) return;
        if(_consoleMonitoringService is null) return;
        if(_consoleMonitoringService.Status == BackgroundServiceStatus.Running) return;
        await _consoleMonitoringService.StartAsync(cancellationToken);
    }

    public async Task StopNotifiersAsync(CancellationToken cancellationToken)
    {
        if(!EnabledFeaturesService.IsFeatureNotifierEnabled) return;
        if(_notifierService is null) return;
        if(_notifierService.Status == BackgroundServiceStatus.Stopped) return;
        await _notifierService.StopAsync(cancellationToken);
    }

    public async Task StopConsoleMonitoringAsync(CancellationToken cancellationToken)
    {
        if(!EnabledFeaturesService.IsFeatureConsoleMonitoringEnabled) return;
        if(_consoleMonitoringService is null) return;
        if(_consoleMonitoringService.Status == BackgroundServiceStatus.Stopped) return;
        await _consoleMonitoringService.StopAsync(cancellationToken);
    }
}