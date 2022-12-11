using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using WALConnector.Types;

namespace WebAutoLogin.Controls.PingStatistics;

internal static class PingStatisticsHelper
{
    private static readonly Color[] ColorCurve;
    private static readonly float[] CurvePoints;
    private static readonly Color ColorMin;
    private static readonly Color ColorMax;
    private static readonly Color ColorDisabled = new() { R = 225, G = 225, B = 225 };

    static PingStatisticsHelper()
    {
        List<float> points = new();
        if (Application.Current.TryFindResource("BarColorCurvePoints") is string curvePoints)
        {
            points
                = curvePoints
                .Split(',')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => float.Parse(x))
                .ToList();
        }
        CurvePoints = points.ToArray();
        ColorDisabled = (Color)Application.Current.TryFindResource("BarColorDisabled");
        ColorMin = (Color)Application.Current.TryFindResource("BarColorMin");
        ColorMax = (Color)Application.Current.TryFindResource("BarColorMax");

        List<Color> colors = new()
        {
            (Color)Application.Current.TryFindResource("BarColor0"),
            (Color)Application.Current.TryFindResource("BarColor1"),
            (Color)Application.Current.TryFindResource("BarColor2"),
            (Color)Application.Current.TryFindResource("BarColor3"),
            (Color)Application.Current.TryFindResource("BarColor4"),
            (Color)Application.Current.TryFindResource("BarColor5")
        };

        ColorCurve = colors.ToArray();
    }

    public static Color GetColor(this float value)
    {
        if (ColorCurve.Length < 2)
            return new Color() { R = 0, G = 0, B = 0 };

        if (value == 0)
            return ColorMin;
        if (value == 1)
            return ColorMax;

        for (int i = 1; i < ColorCurve.Length; i++)
        {
            if(value < CurvePoints[i] && value >= CurvePoints[i-1])
            {
                var diff = (value - CurvePoints[i - 1]) / (CurvePoints[i] - CurvePoints[i - 1]);
                return new()
                {
                    R = (byte)(ColorCurve[i - 1].R * diff + ColorCurve[i].R * (1 - diff)),
                    G = (byte)(ColorCurve[i - 1].G * diff + ColorCurve[i].G * (1 - diff)),
                    B = (byte)(ColorCurve[i - 1].B * diff + ColorCurve[i].B * (1 - diff))
                };
            }
        }
        return ColorDisabled;
    }

    public static byte DetermineLatencyIndex(this long ping, float pingAverage, HostType hostType)
    {
        if (ping < 0)
            return 0;

        // For internet domains
        if (hostType == HostType.Destination)
        {
            if (ping < pingAverage * 0.98)
                return 1;
            if (ping < pingAverage * 1.02)
                return 2;
            if (ping < pingAverage * 1.1)
                return 3;
            if (ping < pingAverage * 1.5)
                return 4;
            if (ping < pingAverage * 2)
                return 5;
            return 6;
        }

        // For LAN/WAN addresses
        if (ping == 0)
            return 1;
        if (ping <= 2)
            return 2;
        if (ping <= 5)
            return 3;
        if (ping <= 10)
            return 4;
        if (ping < 20)
            return 5;
        return 6;
    }


}