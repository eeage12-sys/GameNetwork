using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        TcpListener listener =
            new TcpListener(IPAddress.Loopback, 7777);

        listener.Start();

        Console.WriteLine("클라이언트 연결 대기");

        using TcpClient client =
            await listener.AcceptTcpClientAsync();

        using NetworkStream stream =
            client.GetStream();

        using StreamReader reader =
            new StreamReader(stream);

        using StreamWriter writer =
            new StreamWriter(stream)
            {
                AutoFlush = true
            };

        // 첫 번째 메시지
        string? message1 =
            await reader.ReadLineAsync();

        Console.WriteLine(
            $"서버 수신 1: {message1}");

        await writer.WriteLineAsync("PONG");

        // 두 번째 메시지
        string? message2 =
            await reader.ReadLineAsync();

        Console.WriteLine(
            $"서버 수신 2: {message2}");

        await writer.WriteLineAsync("HELLO_OK");

        listener.Stop();
    }
}