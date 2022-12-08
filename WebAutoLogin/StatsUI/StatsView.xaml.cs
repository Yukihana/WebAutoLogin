using System.Windows;

namespace WebAutoLogin.StatsUI;

/// <summary>
/// Interaction logic for StatsView.xaml
/// </summary>
public partial class StatsView : Window
{
    public StatsView()
    {
        InitializeComponent();
        DataContext = Logic;
    }

    public readonly StatsLogic Logic = new();

    private void Window_Loaded(object sender, RoutedEventArgs e)
        => Logic.Loaded();
}