using AutoLoginConnector.Types;
using System.Collections.Generic;

namespace AutoLoginConnector.Services.Configuration;

public class LoginConfig
{
    public LoginMode AutologinMode { get; set; } = LoginMode.OnDisconnected;

    public string Gateway { get; set; } = "Gateway IP";

    public string Portal { get; set; } = "Login URL";
    public bool MethodIsPost { get; set; } = true;
    public List<KeyValuePair<string, string>> Credentials { get; set; } = new()
    {
        new("user","username"),
        new("pass","password"),
        new("login", "Login")
    };
    public string ValidationString { get; set; } = "Logout";

    public string[] Destinations { get; set; } = new string[] { "https://ping.website.here", "https://and.also.here", "https://and.so.on" };

    public string[] Nodes { get; set; } = new string[] { "https://optional.nics.here/" };

    // Counters

    public int PingPollInterval { get; set; } = 15;
    public int LoginPollMultiplier { get; set; } = 20;
}