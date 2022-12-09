using System;
using System.Linq;

namespace WALConnector.Services.LatencyAnalysis;

internal static class LatencyAnalyser
{
    internal static void Append(this LatencyStatistics data, long roundtripTime, int maxPings)
    {
        // Source
        data.History.Add(roundtripTime);
        while (data.History.Count > maxPings)
            data.History.Remove(0);
        data.Successes = data.History.Where(x => x != -1).ToList();

        // Primary
        data.LastRoundTripTime = roundtripTime;

        // Quality
        data.TotalCount = data.History.Count;
        data.SuccessCount = data.Successes.Count;
        data.FailureCount = data.History.Count - data.SuccessCount;
        data.Stability = data.SuccessCount / (float)data.TotalCount;

        // Analysis
        if (data.Successes.Count > 0)
        {
            data.BestLatency = data.Successes.Min();
            data.WorstLatency = data.Successes.Max();
            data.AverageLatency = MathF.Round((float)data.Successes.Average(), 2);
            data.LatencyJitter = MathF.Round((data.WorstLatency - data.BestLatency) / 2f, 2);
            data.LatencyQuality = MathF.Round(data.LastRoundTripTime / data.AverageLatency, 2);
        }
        else
        {
            data.BestLatency = -1;
            data.WorstLatency = -1;
            data.AverageLatency = -1f;
            data.LatencyJitter = 0f;
            data.LatencyQuality = 0;
        }
    }
}