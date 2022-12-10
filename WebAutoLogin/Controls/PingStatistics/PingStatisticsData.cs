using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;
using WALConnector.Types;

namespace WebAutoLogin.Controls.PingStatistics;

[ObservableObject]
public partial class PingStatisticsData
{
    // Main Panel Info

    [ObservableProperty]
    private string _address = string.Empty;

    [ObservableProperty]
    private string _resolvedAddress = string.Empty;

    [ObservableProperty]
    private HostType _nodeType = HostType.Destination;

    [ObservableProperty]
    private string _lastStatus = string.Empty;

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

    // Quality Colors

    [ObservableProperty]
    private Color _bar1ColorA = new() { R = 240, G = 240, B = 240 };

    [ObservableProperty]
    private Color _bar1ColorB = new() { R = 240, G = 240, B = 240 };

    [ObservableProperty]
    private Color _bar2ColorA = new() { R = 240, G = 240, B = 240 };

    [ObservableProperty]
    private Color _bar2ColorB = new() { R = 240, G = 240, B = 240 };

    [ObservableProperty]
    private Color _bar3ColorA = new() { R = 240, G = 240, B = 240 };

    [ObservableProperty]
    private Color _bar3ColorB = new() { R = 240, G = 240, B = 240 };

    // Quality Tooltip Info (Count, Succeeded, RTO, %stability)

    [ObservableProperty]
    private float _reliability = 0f;

    [ObservableProperty]
    private int _pingTotal = 0;

    [ObservableProperty]
    private int _pingSuccesses = 0;

    [ObservableProperty]
    private int _pingFailures = 0;
}