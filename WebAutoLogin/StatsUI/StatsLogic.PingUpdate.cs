using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using WALConnector.Services.LatencyAnalysis;
using WebAutoLogin.Controls.PingStatistics;

namespace WebAutoLogin.StatsUI;

public partial class StatsLogic
{
    public void PingUpdateThreadSafe()
        => Application.Current.Dispatcher.Invoke(PingUpdate);
    private void PingUpdate()
    {
        if (ConnectorService == null)
            return;

        StatsData.Gateway = UpdatePingInfo(ConnectorService.Data.Gateway);
        StatsData.Portal = UpdatePingInfo(ConnectorService.Data.Portal);

        StatsData.Destinations.Clear();
        ConnectorService.Data.Destinations.ForEach(x =>
        {
            if (UpdatePingInfo(x) is PingStatisticsData data)
                StatsData.Destinations.Add(data);
        });

        StatsData.Nodes.Clear();
        ConnectorService.Data.Nodes.ForEach(x =>
        {
            if (UpdatePingInfo(x) is PingStatisticsData data)
                StatsData.Nodes.Add(data);
        });
    }

    private static PingStatisticsData? UpdatePingInfo(LatencyStatistics? source)
    {
        // Prepare
        if (source == null)
            return null;
        PingStatisticsData target = new();

        // Update
        UpdateStatistics(source, target);

        return target;
    }

    private static void UpdateStatistics(LatencyStatistics source, PingStatisticsData target)
    {
        target.Address = source.Address;
        target.HostType = source.HostType;
        target.LastRoundTripTime = source.LastRoundTripTime;

        target.BestLatency = source.BestLatency;
        target.WorstLatency = source.WorstLatency;
        target.AverageLatency = source.AverageLatency;
        target.LatencyJitter = source.LatencyJitter;

        target.LatencyQualityIndex = DetermineLatencyIndex(source.LastRoundTripTime, source.AverageLatency, source.HostType);

        target.PingTotal = source.TotalCount;
        target.PingSuccesses = source.SuccessCount;
        target.PingFailures = source.TotalCount - source.SuccessCount;
        target.Stability = source.Stability;
    }
}