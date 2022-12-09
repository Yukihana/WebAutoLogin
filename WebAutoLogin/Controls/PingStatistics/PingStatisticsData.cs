using CommunityToolkit.Mvvm.ComponentModel;
using WALConnector.Types;

namespace WebAutoLogin.Controls.PingStatistics;

[ObservableObject]
public partial class PingStatisticsData
{
    // Main Panel Info

    [ObservableProperty]
    private string _address = string.Empty;

    [ObservableProperty]
    private HostType _hostType = HostType.Destination;

    // Latency Info

    [ObservableProperty]
    private long _lastRoundTripTime = -1;

    [ObservableProperty]
    private byte _latencyQualityIndex = 0;

    // Latency Tooltip Info (Average, Min, Max, Jitter)

    [ObservableProperty]
    private long _bestLatency = -1;

    [ObservableProperty]
    private long _worstLatency = -1;

    [ObservableProperty]
    private float _averageLatency = -1f;

    [ObservableProperty]
    private float _latencyJitter = 0f;

    // Quality Info

    [ObservableProperty]
    private byte _stabilityIndex = 0;

    // Quality Tooltip Info (Count, Succeeded, RTO, %stability)

    [ObservableProperty]
    private string _stabilityPercentString = string.Empty;

    [ObservableProperty]
    private int _pingTotal = 0;

    [ObservableProperty]
    private int _pingSuccesses = 0;

    [ObservableProperty]
    private int _pingFailures = 0;
}