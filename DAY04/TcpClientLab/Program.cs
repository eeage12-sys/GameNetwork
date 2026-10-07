using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        using TcpClient client = new();

        await client.ConnectAsync(
            "127.0.0.1",
            7777);

        using NetworkStream stream =
            client.GetStream();

        using StreamReader reader =
            new StreamReader(stream);

        using StreamWriter writer =
            new StreamWriter(stream)
            {
                AutoFlush = true
            };

        // 첫 번째 메시지 전송
        await writer.WriteLineAsync("PING");

        string? response1 =
            await reader.ReadLineAsync();

        Console.WriteLine(
            $"서버 응답 1: {response1}");

        // 두 번째 메시지 전송
        await writer.WriteLineAsync("HELLO");

        string? response2 =
            await reader.ReadLineAsync();

        Console.WriteLine(
            $"서버 응답 2: {response2}");
    }
}