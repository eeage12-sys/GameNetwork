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

        Console.WriteLine("서버 연결 완료");
        Console.WriteLine("메시지를 입력하세요.");
        Console.WriteLine("예: CHAT|안녕하세요");
        Console.WriteLine("종료하려면 /exit");

        while (true)
        {
            string? input =
                Console.ReadLine();

            if (input == null)
            {
                continue;
            }

            if (input == "/exit")
            {
                break;
            }

            await writer.WriteLineAsync(input);

            string? response =
                await reader.ReadLineAsync();

            if (response == null)
            {
                Console.WriteLine(
                    "서버 연결 종료");
                break;
            }

            string[] parts =
                response.Split('|', 2);

            if (parts.Length == 2)
            {
                Console.WriteLine(
                    $"응답 TYPE: {parts[0]}");

                Console.WriteLine(
                    $"응답 VALUE: {parts[1]}");
            }
            else
            {
                Console.WriteLine(
                    $"잘못된 응답: {response}");
            }
        }
    }
}