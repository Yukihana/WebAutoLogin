using System;
using System.Collections.Generic;
using System.Linq;
using WALConnector.Types;

namespace WALConnector.Services.PingStats;

public class PingStatsData
{
    // Service data

    internal List<long> Pings { get; set; } = new();
    internal List<long> SucceededPings { get; set; } = new();

    // Public data

    public string Address { get; set; } = string.Empty;
    public HostType HostType { get; set; } = HostType.Destination;
    public long LastPing => Pings.Count > 0 ? Pings[^1] : -1;

    // Poll Statistics

    public int PingsTotalCount => Pings.Count;
    public int PingsSuccessCount => SucceededPings.Count;

    // Poll Analysis

    public long PingMinimum => SucceededPings.Count > 0 ? SucceededPings.Min() : -1;
    public long PingMaximum => SucceededPings.Count > 0 ? SucceededPings.Max() : -1;
    public double PingAverage => SucceededPings.Count > 0 ? SucceededPings.Average() : -1;
    public double PingJitter => Math.Round((PingMaximum - PingMinimum) / 2d, 2);

    // Grading

    public float Stability => PingsSuccessCount / (float)PingsTotalCount;
}