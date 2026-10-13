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

        IPEndPoint target =
            new IPEndPoint(
                IPAddress.Loopback,
                7778);

        await SendAsync(
            sender,
            target,
            "POSITION|2|4|5");

        await Task.Delay(500);

        await SendAsync(
            sender,
            target,
            "POSITION|4|6|5");

        await Task.Delay(500);

        await SendAsync(
            sender,
            target,
            "POSITION|1|3|5");
    }

    static async Task SendAsync(
        UdpClient sender,
        IPEndPoint target,
        string message)
    {
        byte[] bytes =
            Encoding.UTF8.GetBytes(message);

        await sender.SendAsync(
            bytes,
            bytes.Length,
            target);

        Console.WriteLine($"전송: {message}");
    }
}