using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        using CancellationTokenSource cts = new();

        Task waiting = WaitForConnectionAsync(cts.Token);

        cts.CancelAfter(TimeSpan.FromSeconds(7));

        try
        {
            await waiting;
            Console.WriteLine("접속 완료");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("접속 대기가 취소되었습니다.");
        }
    }

    static async Task WaitForConnectionAsync(
        CancellationToken cancellationToken)
    {
        await Task.Delay(
            TimeSpan.FromSeconds(5),
            cancellationToken);
    }
}