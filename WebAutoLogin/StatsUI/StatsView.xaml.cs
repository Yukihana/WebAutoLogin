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
}