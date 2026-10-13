using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        using TcpClient client = new();

        await client.ConnectAsync(
            "127.0.0.1",
            7777);

        using NetworkStream stream =
            client.GetStream();

        using StreamReader reader =
            new StreamReader(stream, Encoding.UTF8);

        using StreamWriter writer =
            new StreamWriter(stream, Encoding.UTF8)
            {
                AutoFlush = true
            };

        Console.WriteLine("서버 연결 완료");
        Console.WriteLine();

        // 정상 이동 요청 10번
        await RunLoadTestAsync(
            writer,
            reader,
            10);

        Console.WriteLine();

        // 정상 이동 요청 100번
        await RunLoadTestAsync(
            writer,
            reader,
            100);
    }

    static async Task RunLoadTestAsync(
        StreamWriter writer,
        StreamReader reader,
        int requestCount)
    {
        int successCount = 0;
        int errorCount = 0;

        Stopwatch stopwatch =
            Stopwatch.StartNew();

        for (int i = 0; i < requestCount; i++)
        {
            await writer.WriteLineAsync(
                "MOVE|3|5");

            string? response =
                await reader.ReadLineAsync();

            if (response != null &&
                response.StartsWith("MOVE_RESULT|"))
            {
                successCount++;
            }
            else
            {
                errorCount++;
            }
        }

        stopwatch.Stop();

        Console.WriteLine(
            $"===== {requestCount}회 부하 테스트 =====");

        Console.WriteLine(
            $"요청 수: {requestCount}");

        Console.WriteLine(
            $"성공 수: {successCount}");

        Console.WriteLine(
            $"오류 수: {errorCount}");

        Console.WriteLine(
            $"총 시간: {stopwatch.ElapsedMilliseconds} ms");
    }
}