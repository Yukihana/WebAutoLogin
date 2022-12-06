using System.Windows;
using System.Windows.Controls;

namespace WebAutoLogin.Controls.NetworkQualityMeter;

/// <summary>
/// Interaction logic for NetworkQualityMeter.xaml
/// </summary>
public partial class NetworkQualityMeterControl : UserControl
{
    private readonly NetworkQualityMeterLogic Logic = new();

    public NetworkQualityMeterControl()
    {
        InitializeComponent();
        DataContext = Logic;
    }

    public byte Quality
    {
        get { return (byte)GetValue(QualityProperty); }
        set { SetValue(QualityProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty QualityProperty =
        DependencyProperty.Register(nameof(Quality), typeof(byte), typeof(NetworkQualityMeterControl), new PropertyMetadata((byte)0, QualityChanged));

    private static void QualityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        => (d as NetworkQualityMeterControl)?.Logic.SetQuality((byte)e.NewValue);

    /*
    public Color Bar0Color
    {
        get { return (int)GetValue(MyPropertyProperty); }
        set { SetValue(MyPropertyProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty Bar0ColorProperty =
        DependencyProperty.Register("MyProperty", typeof(int), typeof(ownerclass), new PropertyMetadata(0));

    */
}