using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace WebAutoLogin.InterOp;

internal static partial class WinApiHelper
{
    [LibraryImport("iphlpapi.dll", EntryPoint = "GetBestInterface", SetLastError = true)]
    private static partial int GetBestInterface(uint destAddr, out uint bestIfIndex);

    public static IPAddress? GetGatewayForDestination(IPAddress destinationAddress)
    {
        uint destaddr = BitConverter.ToUInt32(destinationAddress.GetAddressBytes(), 0);

        int result = GetBestInterface(destaddr, out uint interfaceIndex);
        if (result != 0)
            throw new Win32Exception(result);

        foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            var niprops = ni.GetIPProperties();
            if (niprops == null)
                continue;

            var gateway = niprops.GatewayAddresses?.FirstOrDefault()?.Address;
            if (gateway == null)
                continue;

            if (ni.Supports(NetworkInterfaceComponent.IPv4))
            {
                var v4props = niprops.GetIPv4Properties();
                if (v4props == null)
                    continue;

                if (v4props.Index == interfaceIndex)
                    return gateway;
            }

            if (ni.Supports(NetworkInterfaceComponent.IPv6))
            {
                var v6props = niprops.GetIPv6Properties();
                if (v6props == null)
                    continue;

                if (v6props.Index == interfaceIndex)
                    return gateway;
            }
        }
        return null;
    }
}