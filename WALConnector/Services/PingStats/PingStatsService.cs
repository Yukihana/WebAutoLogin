namespace WALConnector.Services.PingStats;

// Make this an extension class that uses PingStatsData as the input
internal static class PingStatsService
{
    internal static void UpdateHistory(this PingStatsData data, long roundtripTime, int maxPings)
    {
        data.Pings.Add(roundtripTime);
        while (data.Pings.Count > maxPings)
            data.Pings.Remove(0);
    }
}