using CommunityToolkit.Mvvm.ComponentModel;
using System;
using WALConnector.Types;

namespace WebAutoLogin.StatsUI;

[ObservableObject]
public partial class StatsLogic
{
    internal void Loaded()
        => PingUpdate();

    public void LoginUpdate()
    {
    }

    private static byte DetermineQualityIndex(float pingQuality)
    {
        if (pingQuality == 1)
            return 1;
        if (pingQuality > 0.97)
            return 2;
        if (pingQuality > 0.9)
            return 3;
        if (pingQuality > 0.75)
            return 4;
        if (pingQuality > 0)
            return 5;
        return 0;
    }

    private static byte DetermineLatencyIndex(long ping, double pingAverage, HostType hostType)
    {
        if (ping < 0)
            return 0;

        // For internet domains
        if (hostType == HostType.Destination)
        {
            if (ping < pingAverage * 0.98)
                return 1;
            if (ping < pingAverage * 1.02)
                return 2;
            if (ping < pingAverage * 1.1)
                return 3;
            if (ping < pingAverage * 1.5)
                return 4;
            if (ping < pingAverage * 2)
                return 5;
            return 6;
        }

        // For LAN/WAN addresses
        if (ping == 0)
            return 1;
        if (ping <= 2)
            return 2;
        if (ping <= 5)
            return 3;
        if (ping <= 10)
            return 4;
        if (ping < 20)
            return 5;
        return 6;
    }


}