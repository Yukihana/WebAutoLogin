using WALConnector.Types;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WALConnector.Services.PingStats;

public class PingStatsData
{
    // Service data

    internal PingOptions Options { get; set; } = new();
    internal List<long> Pings { get; set; } = new();
    internal List<long> SucceededPings { get; set; } = new();

    // Public data

    public string Address { get; set; } = string.Empty;
    public HostType HostType { get; set; } = HostType.None;
    public long PingLatency => Pings[^1];

    // Poll Statistics

    public int PingsTotalCount => Pings.Count;
    public int PingsSuccessCount => SucceededPings.Count;

    // Poll Analysis

    public long PingMinimum => SucceededPings.Min();
    public long PingMaximum => SucceededPings.Max();
    public double PingAverage => SucceededPings.Average();
    public double PingJitter => Math.Round((PingMaximum - PingMinimum) / 2d, 2);

    // Grading

    public float Stability => PingsSuccessCount / (float)PingsTotalCount;

    // Obsolete
    public byte StabilityIndex = 0;

    public byte _latencyIndex = 0;
}