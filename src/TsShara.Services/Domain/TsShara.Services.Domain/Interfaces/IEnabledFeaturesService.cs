namespace TsShara.Services.Domain.Interfaces;

public interface IEnabledFeaturesService
{
    bool IsFeatureApiEnabled { get; }
    bool IsFeatureConsoleMonitoringEnabled { get; }
    bool IsFeatureNotifierEnabled { get; }
    bool IsEnabled(string feature);
}