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
    private Color _latencyColor = new() { R = 127, G = 127, B = 127, A = 255 };

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
    private Color _barColorDefault = new() { R = 127, G = 127, B = 127, A = 63 };

    [ObservableProperty]
    private Color _barColorTop = new() { R = 127, G = 127, B = 127, A = 63 };

    [ObservableProperty]
    private Color _barColorBottom = new() { R = 127, G = 127, B = 127, A = 63 };

    [ObservableProperty]
    private bool _bar1Enabled = true;

    [ObservableProperty]
    private bool _bar2Enabled = true;

    [ObservableProperty]
    private bool _bar3Enabled = true;

    // Quality Tooltip Info (Count, Succeeded, RTO, %stability)

    [ObservableProperty]
    private float _reliability = 0f;

    [ObservableProperty]
    private string _reliabilityPercentString = string.Empty;

    [ObservableProperty]
    private int _pingTotal = 0;

    [ObservableProperty]
    private int _pingSuccesses = 0;

    [ObservableProperty]
    private int _pingFailures = 0;
}