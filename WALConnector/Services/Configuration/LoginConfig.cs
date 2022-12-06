using WALConnector.Types;
using System.Collections.Generic;

namespace WALConnector.Services.Configuration;

public class LoginConfig
{
    // IPs

    public string Gateway { get; set; } = "Gateway IP";
    public string Portal { get; set; } = "Login URL";
    public string[] Destinations { get; set; } = new string[] { "https://ping.website.here", "https://and.also.here", "https://and.so.on" };
    public string[] Nodes { get; set; } = new string[] { "https://optional.nics.here/" };

    // Credentials

    public List<KeyValuePair<string, string>> Credentials { get; set; } = new()
    {
        new("user","username"),
        new("pass","password"),
        new("login", "Login")
    };
    public bool LoginMethodIsPost { get; set; } = true;
    public string ValidationString { get; set; } = "ClientID";

    // Ping

    public int PingPollInterval { get; set; } = 15;
    public PingGroupPollingMode DestinationPollingMode { get; set; } = PingGroupPollingMode.SeriallyAlternated;
    public int PingTimeout { get; set; } = 1000;
    public int MaximumPingsCount { get; set; } = 100;

    // Login

    public int LoginPollMultiplier { get; set; } = 20;
    public LoginBehaviour LoginBehaviour { get; set; } = LoginBehaviour.OnDisconnected;
    public int LoginTimeout { get; set; } = 1000;
}