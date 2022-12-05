using System.Windows;

namespace Autologin;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private ProgramContext ProgramContext { get; set; }

    public App() : base()
    {
        ShutdownMode = ShutdownMode.OnExplicitShutdown;
        ProgramContext = new();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
        => ProgramContext.Shutdown();
}