/*
 * 
 * This script was shamelessly stolen by, and then from, ChatGPT
 * 
*/

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

public class NTPTimeFetcher
{
    public static DateTime GetNetworkTime()
    {
        // NTP server address
        const string ntpServer = "time.google.com";

        var ntpData = new byte[48];
        ntpData[0] = 0x1B; // Set protocol version

        var addresses = Dns.GetHostAddresses(ntpServer)
                          .Where(a => a.AddressFamily == AddressFamily.InterNetwork)
                          .ToArray();

        if (addresses.Length == 0)
        {
            throw new Exception("No IPv4 address found for NTP server.");
        }

        var ipEndPoint = new IPEndPoint(addresses[0], 123);

        using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            socket.Connect(ipEndPoint);
            socket.Send(ntpData);
            socket.Receive(ntpData);
            socket.Close();
        }

        // Extract seconds since Jan 1, 1900 (NTP epoch)
        ulong intPart = BitConverter.ToUInt32(ntpData, 40);
        ulong fractPart = BitConverter.ToUInt32(ntpData, 44);

        intPart = SwapEndianness(intPart);
        fractPart = SwapEndianness(fractPart);

        var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

        // Convert to DateTime
        var networkDateTime = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((long)milliseconds);

        return networkDateTime;
    }

    // Swap endianness
    private static uint SwapEndianness(ulong x)
    {
        return (uint)(((x & 0x000000ff) << 24) +
                      ((x & 0x0000ff00) << 8) +
                      ((x & 0x00ff0000) >> 8) +
                      ((x & 0xff000000) >> 24));
    }
}