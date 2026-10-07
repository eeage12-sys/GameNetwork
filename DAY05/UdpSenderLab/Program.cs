using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        using UdpClient sender = new();

        byte[] bytes =
            Encoding.UTF8.GetBytes("POSITION:3,5");

        await sender.SendAsync(
            bytes,
            bytes.Length,
            new IPEndPoint(
                IPAddress.Loopback,
                7778));

        Console.WriteLine(
            "전송: POSITION:3,5");
    }
}