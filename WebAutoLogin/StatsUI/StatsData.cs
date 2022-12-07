using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using WALConnector.Types;

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
    private StatsPingInfo? _gateway = null;

    [ObservableProperty]
    private StatsPingInfo? _portal = null;

    [ObservableProperty]
    private ObservableCollection<StatsPingInfo> _destinations = new();

    [ObservableProperty]
    private ObservableCollection<StatsPingInfo> _nodes = new();

    // Additional Info

    [ObservableProperty]
    private ObservableCollection<string> _availableNICs = new();
}