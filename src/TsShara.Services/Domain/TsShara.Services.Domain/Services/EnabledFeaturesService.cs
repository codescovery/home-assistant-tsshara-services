namespace TsShara.Services.Application.Services;

public class EnabledFeaturesService
{
    public const string FeatureApi = "feature-api";
    public const string FeatureConsoleMonitoring = "feature-console-monitoring";
    public const string FeatureNotifier = "feature-notifier";
    private readonly string[] _args;
    
    public EnabledFeaturesService(string[] args)
    {
        _args = args;
    }
    public bool IsFeatureApiEnabled => IsEnabled(FeatureApi) && !IsFeatureConsoleMonitoringEnabled;
    public bool IsFeatureConsoleMonitoringEnabled => IsEnabled(FeatureConsoleMonitoring) && !IsFeatureApiEnabled;

    public bool IsFeatureNotifierEnabled =>
        IsEnabled(FeatureNotifier) && !IsFeatureApiEnabled && !IsFeatureConsoleMonitoringEnabled;
    public bool IsEnabled(string feature)
    {
        return _args.Contains($"--{feature}");
    }
}