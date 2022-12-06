﻿using System.Linq;
using WALConnector.Services.Configuration;
using WALConnector.Services.Connector;
using WALConnector.Services.PingStats;

namespace WALConnector.Helpers;

internal static class SetupHelper
{
    internal static bool Validate(this ConnectorData data)
    {
        if (string.IsNullOrEmpty(data.Portal?.Address))
            return false;
        if (data.Credentials.Count == 0)
            return false;
        return true;
    }

    internal static void ApplyFrom(this ConnectorData data, LoginConfig config)
    {
        data.Gateway = new() { Address = config.Gateway };
        data.Portal = new() { Address = config.Portal };
        data.Destinations = new(config.Destinations.Select(x => new PingStatsData() { Address = x }));
        data.Nodes = new(config.Nodes.Select(x => new PingStatsData() { Address = x }));

        data.Credentials = config.Credentials;
        data.LoginMethodIsPost = config.LoginMethodIsPost;
        data.ValidationString = config.ValidationString;

        data.PingPollInterval = config.PingPollInterval;
        data.DestinationPollingMode = config.DestinationPollingMode;
        data.PingTimeout = config.PingTimeout;
        data.MaximumPingsCount = config.MaximumPingsCount;

        data.LoginPollMultiplier = config.LoginPollMultiplier;
        data.LoginBehaviour = config.LoginBehaviour;
        data.LoginTimeout = config.LoginTimeout;
    }

    internal static void Initialize(this ConnectorData data)
    {
        data.Options = new()
        {
            MaximumPingsCount = data.MaximumPingsCount,
            Timeout = data.PingTimeout,
        };
        data.AvailableNICs = NetworkHelper.GetNICAddresses();
    }
}