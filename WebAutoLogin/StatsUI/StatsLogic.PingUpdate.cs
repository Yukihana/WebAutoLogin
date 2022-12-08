using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using WALConnector.Services.PingStats;
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
        StatsData.Statistics.Clear();

        if (UpdatePingInfo(ConnectorService.Data.Gateway) is PingStatisticsData gateway)
        {
            StatsData.Statistics.Add(gateway);
        }
        if (UpdatePingInfo(ConnectorService.Data.Portal) is PingStatisticsData portal)
        {
            StatsData.Statistics.Add(portal);
        }
        ConnectorService.Data.Destinations.ForEach(x =>
        {
            if (UpdatePingInfo(x) is PingStatisticsData data)
                StatsData.Statistics.Add(data);
        });
        ConnectorService.Data.Nodes.ForEach(x =>
        {
            if (UpdatePingInfo(x) is PingStatisticsData data)
                StatsData.Statistics.Add(data);
        });

        /*
        StatsData.Gateway = UpdatePingInfo(ConnectorService.Data.Gateway, StatsData.Gateway);
        StatsData.Portal = UpdatePingInfo(ConnectorService.Data.Portal, StatsData.Portal);
        UpdatePingGroup(ConnectorService.Data.Destinations, StatsData.Destinations);
        UpdatePingGroup(ConnectorService.Data.Nodes, StatsData.Nodes);
        */
    }

    private static PingStatisticsData? UpdatePingInfo(PingStatsData? source)
    {
        // Prepare
        if (source == null)
            return null;
        PingStatisticsData target = new();

        // Update
        UpdateStatistics(source, target);

        return target;
    }

    private static void UpdatePingGroup(List<PingStatsData> nodes, ObservableCollection<PingStatisticsData> nodes1)
    {
    }

    private static void UpdateStatistics(PingStatsData source, PingStatisticsData target)
    {
        target.Address = source.Address;
        target.HostType = source.HostType;
        target.LastRoundTripTime = source.LastPing;

        target.PingLowest = source.PingMinimum;
        target.PingHighest = source.PingMaximum;
        target.PingJitter = source.PingJitter;
        target.PingAverage = source.PingAverage;
        target.LatencyIndex = DetermineLatencyIndex(source.LastPing, source.PingAverage, source.HostType);

        target.PingTotal = source.PingsTotalCount;
        target.PingSuccesses = source.PingsSuccessCount;
        target.PingFailures = source.PingsTotalCount - source.PingsSuccessCount;
        target.StabilityPercentString = source.Stability.ToString("0.##");
        target.StabilityIndex = DetermineQualityIndex(source.Stability);
    }
}