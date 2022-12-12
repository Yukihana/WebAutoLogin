using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using WALConnector.Types;

namespace WebAutoLogin.Controls.PingStatistics;

internal static class PingStatisticsHelper
{
    private static readonly Color _latencyFallbackColor;
    private static readonly Color[] _latencyCurveColors;
    private static readonly float[] _remoteLatencyCurve;
    private static readonly long[] _localLatencyCurve;

    private static readonly Color _reliabilityFallbackColor = new() { R = 225, G = 225, B = 225 };
    private static readonly Color[] _reliabilityColors;
    private static readonly float[] _reliabilityCurve;
    private static readonly Color _reliablilityFloorColor;
    private static readonly Color _reliabilityCeilingColor;

    static PingStatisticsHelper()
    {
        // Latency

        _latencyFallbackColor = (Color)Application.Current.TryFindResource("LatencyFallbackColor");
        _remoteLatencyCurve = ((CurveRemote)Application.Current.TryFindResource("RemoteLatencyCurve")).ToArray();
        _localLatencyCurve = ((CurveLocal)Application.Current.TryFindResource("LocalLatencyCurve")).ToArray();
        _latencyCurveColors = ((Palette)Application.Current.TryFindResource("LatencyCurveColors")).ToArray();

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
        _reliabilityCurve = points.ToArray();
        _reliabilityFallbackColor = (Color)Application.Current.TryFindResource("BarColorDisabled");
        _reliablilityFloorColor = (Color)Application.Current.TryFindResource("BarColorMin");
        _reliabilityCeilingColor = (Color)Application.Current.TryFindResource("BarColorMax");

        // Curves

        _reliabilityColors = new List<Color>()
        {
            (Color)Application.Current.TryFindResource("BarColor0"),
            (Color)Application.Current.TryFindResource("BarColor1"),
            (Color)Application.Current.TryFindResource("BarColor2"),
            (Color)Application.Current.TryFindResource("BarColor3"),
            (Color)Application.Current.TryFindResource("BarColor4"),
            (Color)Application.Current.TryFindResource("BarColor5")
        }.ToArray();
    }

    public static Color GetColor(this float value)
    {
        if (_reliabilityColors.Length < 2)
            return new Color() { R = 0, G = 0, B = 0 };

        if (value == 0)
            return _reliablilityFloorColor;
        if (value == 1)
            return _reliabilityCeilingColor;

        for (int i = 1; i < _reliabilityColors.Length; i++)
        {
            if (value < _reliabilityCurve[i] && value >= _reliabilityCurve[i - 1])
            {
                var diff = (value - _reliabilityCurve[i - 1]) / (_reliabilityCurve[i] - _reliabilityCurve[i - 1]);
                return new()
                {
                    R = (byte)(_reliabilityColors[i - 1].R * diff + _reliabilityColors[i].R * (1 - diff)),
                    G = (byte)(_reliabilityColors[i - 1].G * diff + _reliabilityColors[i].G * (1 - diff)),
                    B = (byte)(_reliabilityColors[i - 1].B * diff + _reliabilityColors[i].B * (1 - diff))
                };
            }
        }
        return _reliabilityFallbackColor;
    }

    public static Color GetRemoteLatencyGradeColor(this long pingTime, float average)
    {
        var grade = pingTime / (float)average;
        return grade.GetColorF(_latencyCurveColors, _remoteLatencyCurve, _latencyFallbackColor);
    }

    public static Color GetLocalLatencyGradeColor(this long pingTime)
    {
        return pingTime.GetColor(_latencyCurveColors, _localLatencyCurve, _latencyFallbackColor);
    }

    private static Color GetColor(this long grade, Color[] colors, long[] curve, Color fallback, Color? min = null, Color? max = null)
    {
        if (curve.Length != colors.Length || curve.Length < 2)
            return fallback;

        if (grade <= curve[0])
            return min == null ? colors[0] : min.Value;
        if (grade >= curve[^1])
            return max == null ? colors[^1] : max.Value;

        for (int i = 1; i < curve.Length; i++)
        {
            if (grade >= curve[i - 1] && grade < curve[i])
            {
                var diff = (grade - curve[i - 1]) / (curve[i] - curve[i - 1]);
                return new()
                {
                    R = (byte)(colors[i - 1].R * diff + colors[i].R * (1 - diff)),
                    G = (byte)(colors[i - 1].G * diff + colors[i].G * (1 - diff)),
                    B = (byte)(colors[i - 1].B * diff + colors[i].B * (1 - diff))
                };
            }
        }

        return fallback;
    }

    private static Color GetColorF(this float grade, Color[] colors, float[] curve, Color fallback, Color? min = null, Color? max = null)
    {
        if (curve.Length != colors.Length || curve.Length < 2)
            return fallback;

        if (grade <= curve[0])
            return min == null ? colors[0] : min.Value;
        if (grade >= curve[^1])
            return max == null ? colors[^1] : max.Value;

        for (int i = 1; i < curve.Length; i++)
        {
            if (grade < curve[i] && grade >= curve[i - 1])
            {
                var diff = (grade - curve[i - 1]) / (curve[i] - curve[i - 1]);
                return new()
                {
                    R = (byte)(colors[i - 1].R * diff + colors[i].R * (1 - diff)),
                    G = (byte)(colors[i - 1].G * diff + colors[i].G * (1 - diff)),
                    B = (byte)(colors[i - 1].B * diff + colors[i].B * (1 - diff))
                };
            }
        }

        return fallback;
    }
}