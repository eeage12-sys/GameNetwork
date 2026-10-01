using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        using CancellationTokenSource cts = new();

        // 전체 작업은 3초 안에 완료되어야 함
        cts.CancelAfter(TimeSpan.FromSeconds(3));

        try
        {
            // 서로 독립적인 두 작업을 먼저 시작
            Task<string> profileTask =
                LoadProfileAsync(cts.Token);

            Task<string> inventoryTask =
                LoadInventoryAsync(cts.Token);

            // 두 작업이 모두 끝날 때까지 함께 기다림
            string[] results = await Task.WhenAll(
                profileTask,
                inventoryTask);

            Console.WriteLine(results[0]);
            Console.WriteLine(results[1]);
            Console.WriteLine("모든 작업 완료");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("작업이 취소되었습니다.");
        }
        catch (Exception error)
        {
            Console.WriteLine("오류 발생: " + error.Message);
        }
    }

    static async Task<string> LoadProfileAsync(
        CancellationToken cancellationToken)
    {
        await Task.Delay(
            TimeSpan.FromSeconds(1),
            cancellationToken);

        return "프로필 로드 완료";
    }

    static async Task<string> LoadInventoryAsync(
        CancellationToken cancellationToken)
    {
        await Task.Delay(
            TimeSpan.FromSeconds(2),
            cancellationToken);

        return "인벤토리 로드 완료";
    }
}