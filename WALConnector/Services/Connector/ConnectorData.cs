using System.Collections.Generic;
using System.Net.Http;
using WALConnector.Services.PingStats;
using WALConnector.Types;

namespace WALConnector.Services.Connector;

internal class ConnectorData
{
    public PingOptions? Options { get; set; } = new();
    internal bool ConfigChanged { get; set; } = false;
    internal List<string> AvailableNICs { get; set; } = new();

    internal PingStatsData? Gateway { get; set; }
    internal PingStatsData? Portal { get; set; }
    internal List<PingStatsData> Destinations { get; set; } = new();
    internal List<PingStatsData> Nodes { get; set; } = new();

    internal List<KeyValuePair<string, string>> Credentials { get; set; } = new();
    internal FormUrlEncodedContent EncodedParams { get; set; } = new(new List<KeyValuePair<string, string>>());
    internal bool LoginMethodIsPost { get; set; } = true;
    internal string ValidationString { get; set; } = string.Empty;

    internal int PingPollInterval { get; set; } = 15;
    internal PingGroupPollingMode DestinationPollingMode { get; set; } = PingGroupPollingMode.SeriallyAlternated;
    internal int PingTimeout { get; set; } = 1000;
    internal int MaximumPingsCount { get; set; } = 100;

    internal int LoginPollMultiplier { get; set; } = 20;
    internal LoginBehaviour LoginBehaviour { get; set; } = LoginBehaviour.OnDisconnected;
    internal int LoginTimeout { get; set; } = 1000;
}