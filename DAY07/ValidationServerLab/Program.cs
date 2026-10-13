using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        TcpListener listener =
            new TcpListener(IPAddress.Loopback, 7777);

        listener.Start();

        Console.WriteLine("DAY07 검증 서버 시작");
        Console.WriteLine("클라이언트 연결 대기");

        using TcpClient client =
            await listener.AcceptTcpClientAsync();

        using NetworkStream stream =
            client.GetStream();

        using StreamReader reader =
            new StreamReader(stream, Encoding.UTF8);

        using StreamWriter writer =
            new StreamWriter(stream, Encoding.UTF8)
            {
                AutoFlush = true
            };

        Console.WriteLine("클라이언트 연결 완료");

        while (true)
        {
            string? message =
                await reader.ReadLineAsync();

            if (message == null)
            {
                Console.WriteLine("클라이언트 연결 종료");
                break;
            }

            Console.WriteLine($"수신: {message}");

            string[] parts =
                message.Split('|');

            // MOVE|x|y 형식이 아니면 오류
            if (parts.Length != 3 ||
                parts[0] != "MOVE")
            {
                await writer.WriteLineAsync(
                    "ERROR|INVALID_FORMAT");

                continue;
            }

            bool xSuccess =
                int.TryParse(parts[1], out int x);

            bool ySuccess =
                int.TryParse(parts[2], out int y);

            // 숫자로 변환할 수 없는 경우
            if (!xSuccess || !ySuccess)
            {
                await writer.WriteLineAsync(
                    "ERROR|INVALID_FORMAT");

                continue;
            }

            // 0~10 범위를 벗어난 경우
            if (x < 0 || x > 10 ||
                y < 0 || y > 10)
            {
                await writer.WriteLineAsync(
                    "ERROR|OUT_OF_RANGE");

                continue;
            }

            // 정상 이동
            await writer.WriteLineAsync(
                $"MOVE_RESULT|{x}|{y}");
        }

        listener.Stop();
    }
}