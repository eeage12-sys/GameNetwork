using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Task<string> profileTask = LoadProfileAsync();
        Task<string> inventoryTask = LoadInventoryAsync();

        string[] results = await Task.WhenAll(
            profileTask,
            inventoryTask);

        Console.WriteLine(results[0]);
        Console.WriteLine(results[1]);
    }

    static async Task<string> LoadProfileAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        return "프로필 로드 완료";
    }

    static async Task<string> LoadInventoryAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
        return "인벤토리 로드 완료";
    }
}