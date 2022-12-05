using AutoLoginConnector.Services.Connector;

namespace Autologin.StatsUI;

public partial class StatsLogic
{
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
                _connectorService.OnPingsPolled = PingUpdate;
                _connectorService.OnLoginSucceeded = LoginUpdate;
            }
        }
    }

    private readonly StatsData _statsData = new();
    public StatsData StatsData => _statsData;

    public void PingUpdate()
    {

    }
    public void LoginUpdate()
    {

    }

    public void Update() { }
}