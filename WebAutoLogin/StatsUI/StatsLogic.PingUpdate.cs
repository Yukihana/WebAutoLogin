using System.Collections.Generic;
using System.Collections.ObjectModel;
using WALConnector.Services.PingStats;
using WebAutoLogin.Controls.PingStatistics;

namespace WebAutoLogin.StatsUI;

public partial class StatsLogic
{
    public void PingUpdate()
    {
        if (ConnectorService == null)
            return;
        StatsData.Gateway = UpdatePingInfo(ConnectorService.Data.Gateway, StatsData.Gateway);
        StatsData.Portal = UpdatePingInfo(ConnectorService.Data.Portal, StatsData.Portal);
        UpdatePingGroup(ConnectorService.Data.Destinations, StatsData.Destinations);
        UpdatePingGroup(ConnectorService.Data.Nodes, StatsData.Nodes);
    }

    private static PingStatisticsData? UpdatePingInfo(PingStatsData? source, PingStatisticsData? target)
    {
        // Prepare
        if (source == null)
            return null;
        target ??= new();

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
        target.HostType= source.HostType;
        target.LastRoundTripTime = source.LastPing;

        target.PingTotal = source.PingsTotalCount;
        target.PingSuccesses = source.PingsSuccessCount;
        target.PingFailures = source.PingsTotalCount - source.PingsSuccessCount;

    }
}