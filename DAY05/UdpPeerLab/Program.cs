using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static int sentCount = 0;
    static int receivedCount = 0;

    static async Task Main(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine(
                "사용법: dotnet run -- <내 포트> <상대 포트> <이름>");
            return;
        }

        int myPort = int.Parse(args[0]);
        int remotePort = int.Parse(args[1]);
        string name = args[2];

        using UdpClient peer = new(myPort);
        using CancellationTokenSource cts = new();

        IPEndPoint remoteEndPoint =
            new IPEndPoint(IPAddress.Loopback, remotePort);

        Console.WriteLine(
            $"[{name}] 시작 - 내 포트: {myPort}, 상대 포트: {remotePort}");

        Console.WriteLine(
            "위치 메시지 예: POSITION:3,5");

        Console.WriteLine(
            "종료하려면 /exit");

        Task receiveTask =
            ReceiveLoopAsync(peer, cts.Token);

        while (true)
        {
            string? input = Console.ReadLine();

            if (input == null)
                continue;

            if (input.Equals(
                "/exit",
                StringComparison.OrdinalIgnoreCase))
            {
                cts.Cancel();
                break;
            }

            string message =
                $"[{name}] {input}";

            byte[] bytes =
                Encoding.UTF8.GetBytes(message);

            await peer.SendAsync(
                bytes,
                bytes.Length,
                remoteEndPoint);

            sentCount++;

            Console.WriteLine(
                $"전송: {message}");

            Console.WriteLine(
                $"보낸 메시지: {sentCount}개 / 받은 메시지: {receivedCount}개");
        }

        try
        {
            await receiveTask;
        }
        catch (OperationCanceledException)
        {
        }

        Console.WriteLine(
            $"최종 전송: {sentCount}개 / 최종 수신: {receivedCount}개");

        Console.WriteLine(
            $"[{name}] 종료");
    }

    static async Task ReceiveLoopAsync(
        UdpClient peer,
        CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                UdpReceiveResult result =
                    await peer.ReceiveAsync(cancellationToken);

                string message =
                    Encoding.UTF8.GetString(result.Buffer);

                receivedCount++;

                Console.WriteLine(
                    $"수신 {result.RemoteEndPoint}: {message}");

                Console.WriteLine(
                    $"보낸 메시지: {sentCount}개 / 받은 메시지: {receivedCount}개");
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }
}