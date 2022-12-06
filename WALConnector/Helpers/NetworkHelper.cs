using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace WALConnector.Helpers;

internal static class NetworkHelper
{
    internal static List<string> GetNICAddresses()
    {
        List<string> result = new();
        foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                nic.OperationalStatus == OperationalStatus.Up)
            {
                foreach (var ipInfo in nic.GetIPProperties().UnicastAddresses)
                {
                    if (ipInfo.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork ||
                        ipInfo.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        result.Add(ipInfo.Address.ToString());
                    }
                }
            }
        }
        return result;
    }
}