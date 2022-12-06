using WebAutoLogin.StatsUI;
using WebAutoLogin.TrayHandler;
using WALConnector.Services.Connector;
using System.Threading.Tasks;
using System.Windows;

namespace WebAutoLogin;

internal class WALContext
{
    private readonly ConnectorService _connectorService;
    private readonly TrayAgent _trayAgent;
    private StatsView? _view;

    public WALContext()
    {
        _connectorService = new();
        _trayAgent = new();

        Task.Run(InitializeAsync);
    }

    // Setup services async
    internal async Task InitializeAsync()
    {
        var success = await _connectorService.Start();
        if (success)
            await Application.Current.Dispatcher.BeginInvoke(CompleteInitialization);
        else
        {
            await Application.Current.Dispatcher.BeginInvoke(() =>
            {
                FirstRun();
                Shutdown();
            });
        }
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
    internal static void FirstRun()
    {
        MessageBox.Show("Unable to load config. This is probably the first run or the configuration is invalid.", "Startup Aborted", MessageBoxButton.OK, MessageBoxImage.Error);
    }
    internal void Shutdown()
    {
        Task.Run(_connectorService.Stop);
        _view = null;

        _trayAgent.Unload();
        Application.Current.Shutdown();
    }
}