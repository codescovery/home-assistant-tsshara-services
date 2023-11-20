namespace TsShara.Services.Domain.Interfaces;

public interface IFeatureService
{
    IEnabledFeaturesService EnabledFeaturesService { get; }
    Task StartNotifiersAsync(CancellationToken cancellationToken);
    Task StartConsoleMonitoringAsync(CancellationToken cancellationToken);
    Task StopNotifiersAsync(CancellationToken cancellationToken);
    Task StopConsoleMonitoringAsync(CancellationToken cancellationToken);

}