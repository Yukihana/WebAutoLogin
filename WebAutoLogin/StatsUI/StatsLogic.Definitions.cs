using WALConnector.Services.Connector;

namespace WebAutoLogin.StatsUI;

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
                _connectorService.OnPingsPolled = PingUpdateThreadSafe;
                _connectorService.OnLoginSucceeded = LoginUpdate;
            }
        }
    }

    // Data Model

    private readonly StatsData _statsData = new();
    public StatsData StatsData => _statsData;
}