using CommunityToolkit.Mvvm.ComponentModel;

namespace WebAutoLogin.Controls.NetworkQualityMeter;

[ObservableObject]
internal partial class NetworkQualityMeterLogic
{
    internal void SetQuality(byte v)
    {
        Bar1ColorIndex = v >= 1 ? v : byte.MinValue;
        Bar2ColorIndex = v >= 2 ? v : byte.MinValue;
        Bar3ColorIndex = v >= 3 ? v : byte.MinValue;
    }

    [ObservableProperty]
    private byte _bar1ColorIndex = 1;

    [ObservableProperty]
    private byte _bar2ColorIndex = 2;

    [ObservableProperty]
    private byte _bar3ColorIndex = 3;
}