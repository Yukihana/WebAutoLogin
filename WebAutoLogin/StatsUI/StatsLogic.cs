using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using WALConnector.Services.Connector;

namespace WebAutoLogin.StatsUI;

[ObservableObject]
public partial class StatsLogic
{
    // Connector Service

    private ConnectorService? _connectorService;

    public ConnectorService? ConnectorService
    {
        get => _connectorService;
        set
        {
            _connectorService?.Decouple();
            _connectorService = value;
            if (_connectorService != null)
            {
                _connectorService.OnStarted = Loaded;
                _connectorService.OnProgressChanged = ProgressChanged;
                _connectorService.OnPingsPolled = PingUpdateThreadSafe;
                _connectorService.OnLoginAttempted = LoginAttempted;
                _connectorService.OnLoginSucceeded = LoginUpdate;
            }
        }
    }

    // Data Model

    private readonly StatsData _statsData = new();
    public StatsData StatsData => _statsData;

    // Initialisation

    internal void Loaded()
    => PingUpdate();

    public void LoginAttempted()
    {
    }
    public void LoginUpdate()
    {
    }


}