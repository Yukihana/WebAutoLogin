using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using WALConnector.Types;
using WebAutoLogin.Controls.PingStatistics;

namespace WebAutoLogin.StatsUI;

[ObservableObject]
public partial class StatsData
{
    // Quick Buttons

    [ObservableProperty]
    private bool _IsLoggedIn = new();

    [ObservableProperty]
    private LoginBehaviour _loginBehaviour = LoginBehaviour.OnDisconnected;

    // Slider Pane Controls (Future Update)

    [ObservableProperty]
    private int _pingInterval = 15;

    [ObservableProperty]
    private int _loginMultiplier = 20;

    // Ping Statistics

    [ObservableProperty]
    private PingStatisticsData? _gateway = null;

    [ObservableProperty]
    private PingStatisticsData? _portal = null;

    [ObservableProperty]
    private ObservableCollection<PingStatisticsData> _destinations = new();

    [ObservableProperty]
    private ObservableCollection<PingStatisticsData> _nodes = new();

    // Additional Info

    [ObservableProperty]
    private ObservableCollection<string> _availableNICs = new();
}