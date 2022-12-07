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

    [ObservableProperty]
    private long _lastRoundTripTime = -1;

    [ObservableProperty]
    private byte _latencyIndex = 0;

    [ObservableProperty]
    private byte _stabilityIndex = 0;

    // Ping Tooltip Info (Average, Min, Max, Jitter)

    [ObservableProperty]
    private long _pingLowest = -1;

    [ObservableProperty]
    private long _pingHighest = -1;

    [ObservableProperty]
    private long _pingJitter = 0;

    [ObservableProperty]
    private long _pingAverage = -1;

    // Quality Tooltip Info (Count, Succeeded, RTO, %stability)

    [ObservableProperty]
    private int _pingTotal = 0;

    [ObservableProperty]
    private int _pingSuccesses = 0;

    [ObservableProperty]
    private int _pingFailures = 0;

    [ObservableProperty]
    private string _stabilityPercentString = string.Empty;
}