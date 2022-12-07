using System.Collections.Generic;
using System.Net.Http;
using WALConnector.Services.PingStats;
using WALConnector.Types;

namespace WALConnector.Services.Connector;

public class ConnectorData
{
    public bool ConfigChanged { get; internal set; } = false;
    public List<string> AvailableNICs { get; internal set; } = new();
    public int DestinationIndex { get; internal set; } = 0;

    public PingStatsData? Gateway { get; internal set; }
    public PingStatsData? Portal { get; internal set; }
    public List<PingStatsData> Destinations { get; internal set; } = new();
    public List<PingStatsData> Nodes { get; internal set; } = new();

    public List<KeyValuePair<string, string>> Credentials { get; internal set; } = new();
    public FormUrlEncodedContent EncodedParams { get; internal set; } = new(new List<KeyValuePair<string, string>>());
    public bool LoginMethodIsPost { get; internal set; } = true;
    public string ValidationString { get; internal set; } = string.Empty;

    public int PingPollInterval { get; internal set; } = 15;
    public PingGroupPollingMode DestinationPollingMode { get; internal set; } = PingGroupPollingMode.SeriallyAlternated;
    public int PingTimeout { get; internal set; } = 1000;
    public int MaximumPingsCount { get; internal set; } = 100;

    public int LoginPollMultiplier { get; internal set; } = 20;
    public LoginBehaviour LoginBehaviour { get; internal set; } = LoginBehaviour.OnDisconnected;
    public int LoginTimeout { get; internal set; } = 1000;
}