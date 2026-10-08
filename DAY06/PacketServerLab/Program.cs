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
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        TcpListener listener =
            new TcpListener(IPAddress.Loopback, 7777);

        listener.Start();

        Console.WriteLine("패킷 서버 시작");
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

            if (string.IsNullOrWhiteSpace(message))
            {
                await writer.WriteLineAsync(
                    "ERROR|빈 메시지입니다");

                continue;
            }

            string[] parts =
                message.Split('|');

            string type = parts[0];

            // CHAT 처리
            if (type == "CHAT")
            {
                if (parts.Length < 2)
                {
                    await writer.WriteLineAsync(
                        "ERROR|채팅 내용이 없습니다");

                    continue;
                }

                string value =
                    string.Join("|", parts, 1, parts.Length - 1);

                Console.WriteLine(
                    $"채팅 수신: {value}");

                await writer.WriteLineAsync(
                    "SYSTEM|메시지를 받았습니다");
            }

            // MOVE 처리
            else if (type == "MOVE")
            {
                if (parts.Length != 3)
                {
                    await writer.WriteLineAsync(
                        "MOVE_ERROR|좌표 형식 오류");

                    continue;
                }

                bool xSuccess =
                    int.TryParse(parts[1], out int x);

                bool ySuccess =
                    int.TryParse(parts[2], out int y);

                if (!xSuccess || !ySuccess)
                {
                    await writer.WriteLineAsync(
                        "MOVE_ERROR|좌표 형식 오류");

                    continue;
                }

                Console.WriteLine(
                    $"이동 요청: X={x}, Y={y}");

                await writer.WriteLineAsync(
                    $"MOVE_RESULT|{x}|{y}");
            }

            // 알 수 없는 메시지
            else
            {
                Console.WriteLine(
                    $"알 수 없는 타입: {type}");

                await writer.WriteLineAsync(
                    "ERROR|알 수 없는 메시지 타입");
            }
        }

        listener.Stop();
    }
}