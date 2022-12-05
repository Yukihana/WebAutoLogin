using AutoLoginConnector.Services.Configuration;
using AutoLoginConnector.Services.Connector;
using AutoLoginConnector.Services.PingStats;
using System.Linq;

namespace Autologin.Helpers;

internal static class ConnectorHelper
{
    internal static void ApplyTo(this LoginConfig config, ConnectorData data)
    {
        data.LoginMode = config.AutologinMode;
        data.Gateway = new() { Address = config.Gateway };
        data.Portal = new() { Address = config.Portal };
        data.Credentials = config.Credentials;
        data.Destinations = new(config.Destinations.Select(x => new PingStatsData() { Address = x }));
        data.CustomNICs = new(config.Nodes.Select(x => new PingStatsData() { Address = x }));
    }
    internal static bool Validate(this ConnectorData data)
    {
        if (string.IsNullOrEmpty(data.Portal?.Address))
            return false;
        if (data.Credentials.Count == 0)
            return false;
        return true;
    }
}