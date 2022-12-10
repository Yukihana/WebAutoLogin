using WALConnector.Types;

namespace WebAutoLogin.Controls.PingStatistics;

internal static class PingStatisticsHelper
{
    public static byte DetermineLatencyIndex(this long ping, float pingAverage, HostType hostType)
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