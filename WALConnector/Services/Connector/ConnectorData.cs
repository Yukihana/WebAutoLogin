using AutoLoginConnector.Services.PingStats;
using AutoLoginConnector.Types;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;

namespace AutoLoginConnector.Services.Connector;

internal class ConnectorData
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public PingOptions? Options { get; set; } = new();



    private LoginMode loginMode = LoginMode.OnDisconnected;
    private PingStatsData? gateway;
    private PingStatsData? portal;
    private List<PingStatsData> destinations = new();
    private List<PingStatsData> customNICs = new();
    private List<string> availableNICs = new();
    private List<KeyValuePair<string, string>> credentials = new();
    private FormUrlEncodedContent encodedParams = new(new List<KeyValuePair<string, string>>());
    private bool methodIsPost = true;
    private string validationString = string.Empty;
    private bool configChanged = false;
    private int pingInterval = 15;
    private int loginMultiplier = 20;





    internal LoginMode LoginMode { get => loginMode; set => loginMode = value; }
    internal PingStatsData? Gateway { get => gateway; set => gateway = value; }
    internal PingStatsData? Portal { get => portal; set => portal = value; }
    internal List<PingStatsData> Destinations { get => destinations; set => destinations = value; }
    internal List<PingStatsData> CustomNICs { get => customNICs; set => customNICs = value; }
    internal List<string> AvailableNICs { get => availableNICs; set => availableNICs = value; }
    internal List<KeyValuePair<string, string>> Credentials { get => credentials; set => credentials = value; }
    internal FormUrlEncodedContent EncodedParams { get => encodedParams; set => encodedParams = value; }
    internal bool LoginIsMethodPost { get => methodIsPost; set => methodIsPost = value; }
    internal string ValidationString { get => validationString; set => validationString = value; }
    internal bool ConfigChanged { get => configChanged; set => configChanged = value; }
    internal int PingInterval { get => pingInterval; set => pingInterval = value; }
    internal int LoginMultiplier { get => loginMultiplier; set=> loginMultiplier = value; }
}
