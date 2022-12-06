using WebAutoLogin.StatsUI;
using WebAutoLogin.TrayHandler;
using WALConnector.Services.Connector;
using System.Threading.Tasks;
using System.Windows;

namespace WebAutoLogin;

internal class ProgramContext
{
    private readonly ConnectorService _connectorService;
    private readonly TrayAgent _trayAgent;
    private StatsView? _view;

    public ProgramContext()
    {
        _connectorService = new();
        _trayAgent = new();

        Task.Run(InitializeAsync);
    }

    // Setup services async
    internal async Task InitializeAsync()
    {
        await _connectorService.Initialize();
        await Application.Current.Dispatcher.BeginInvoke(CompleteInitialization);
    }

    // Initialize UI here
    internal void CompleteInitialization()
    {
        AttachUI();
    }

    internal void AttachUI()
    {
        if(_view == null)
        {
            _view = new();
            _view.Logic.ConnectorService = _connectorService;
            _view.Show();
            _view.Closed += (sender, args) => Shutdown();
        }
    }

    internal void Shutdown()
    {
        _connectorService.TryEnd();
        _view = null;

        _trayAgent.Unload();
        Application.Current.Shutdown();
    }
}