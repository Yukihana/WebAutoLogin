using Autologin.Controls.PingerStatus;
using Autologin.Types;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Autologin.StatsUI;

[ObservableObject]
public partial class StatsData
{
    // Login

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _isPortalUp = false;

    [ObservableProperty]
    private bool _isLoggedIn = false;

    [ObservableProperty]
    private ConnectionState _connectionState = ConnectionState.Disconnected;

    // Routing

    [ObservableProperty]
    private PingerStatusControlData _gateway = new();

    [ObservableProperty]
    private PingerStatusControlData _portal = new();

    [ObservableProperty]
    private ObservableCollection<PingerStatusControlData> _selfAddresses = new();

    [ObservableProperty]
    private ObservableCollection<PingerStatusControlData> _internetAddresses = new();
}