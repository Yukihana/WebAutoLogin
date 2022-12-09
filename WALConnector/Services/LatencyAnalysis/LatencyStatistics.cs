using System.Collections.Generic;
using WALConnector.Types;

namespace WALConnector.Services.LatencyAnalysis;

public class LatencyStatistics
{
    // Source

    internal List<long> History { get; set; } = new();
    internal List<long> Successes { get; set; } = new();

    // Primary

    public string Address { get; internal set; } = string.Empty;
    public HostType HostType { get; internal set; } = HostType.Destination;
    public long LastRoundTripTime { get; internal set; } = -1;

    // Quality

    public int TotalCount { get; internal set; } = 0;
    public int SuccessCount { get; internal set; } = 0;
    public int FailureCount { get; internal set; }
    public float Stability { get; internal set; } = 0f;

    // Analysis

    public long BestLatency { get; internal set; } = -1;
    public long WorstLatency { get; internal set; } = -1;
    public float AverageLatency { get; internal set; } = -1f;
    public float LatencyJitter { get; internal set; } = 0f;
    public float LatencyQuality { get; internal set; } = 0f;
}