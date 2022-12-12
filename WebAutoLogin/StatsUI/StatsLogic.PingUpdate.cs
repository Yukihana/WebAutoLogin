using System.Windows;
using WALConnector.Helpers;
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
        target.ResolvedAddress = source.ResolvedAddress;
        target.NodeType = source.NodeType;
        target.LastRoundTripTime = source.LastRoundTripTime;
        target.LastStatus = source.LastStatus;

        target.BestLatency = source.BestLatency;
        target.WorstLatency = source.WorstLatency;
        target.AverageLatency = source.AverageLatency;
        target.LatencyJitter = source.LatencyJitter;

        target.LatencyColor = source.IsDestination
            ? source.LastRoundTripTime.GetRemoteLatencyGradeColor(source.AverageLatency)
            : source.LastRoundTripTime.GetLocalLatencyGradeColor();

        target.PingTotal = source.TotalCount;
        target.PingSuccesses = source.SuccessCount;
        target.PingFailures = source.TotalCount - source.SuccessCount;
        target.Reliability = source.Reliability;
        target.ReliabilityPercentString = $"{source.Reliability * 100}%";

        var barColor = source.Reliability.GetColor().DeepClone();
        target.BarColorTop = barColor.DeepClone();
        barColor.A = (byte)(byte.MaxValue * 0.5f);
        target.BarColorBottom = barColor;

        target.Bar1Enabled = source.Reliability > 0.9;
        target.Bar2Enabled = source.Reliability > 0.75;
        target.Bar3Enabled = source.Reliability > 0.5;
    }
}