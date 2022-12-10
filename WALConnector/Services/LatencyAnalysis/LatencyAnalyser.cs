using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace WALConnector.Services.LatencyAnalysis;

internal static class LatencyAnalyser
{
    internal static void Append(this LatencyStatistics data, PingReply reply, int maxPings)
    {
        data.LastStatus = reply.Status.ToString();
        var roundTripTime
            = reply.Status == IPStatus.Success
            ? reply.RoundtripTime
            : -1;
        data.ResolvedAddress = reply.Address.ToString();

        // Source
        data.History.Add(roundTripTime);
        while (data.History.Count > maxPings)
            data.History.RemoveAt(0);
        data.Successes = data.History.Where(x => x != -1).ToList();

        // Primary
        data.LastRoundTripTime = roundTripTime;

        // Quality
        data.TotalCount = data.History.Count;
        data.SuccessCount = data.Successes.Count;
        data.FailureCount = data.TotalCount - data.SuccessCount;
        data.Reliability = MathF.Round(data.SuccessCount / (float)data.TotalCount, 2);

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