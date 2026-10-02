using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class Program
{
    private const string ServerUrl = "https://localhost:5001";

    public static void Main(string[] args)
    {
        RunAsync().GetAwaiter().GetResult();
    }

    private static async Task RunAsync()
    {
        Console.Write("아이디: ");
        string id = Console.ReadLine();

        Console.Write("비밀번호: ");
        string password = Console.ReadLine();

        LoginRequest loginRequest = new LoginRequest();
        loginRequest.Id = id;
        loginRequest.Password = password;

        string requestJson =
            JsonSerializer.Serialize(loginRequest);

        using (HttpClient client = new HttpClient())
        using (StringContent content = new StringContent(
            requestJson,
            Encoding.UTF8,
            "application/json"))
        {
            HttpResponseMessage response =
                await client.PostAsync(
                    ServerUrl + "/login",
                    content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(
                    "로그인 실패: " +
                    response.StatusCode);

                return;
            }

            string responseJson =
                await response.Content.ReadAsStringAsync();

            LoginResponse loginResponse =
                JsonSerializer.Deserialize<LoginResponse>(
                    responseJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            Console.WriteLine("로그인 성공");
            Console.WriteLine(
                "PlayerId: " +
                loginResponse.PlayerId);

            Console.WriteLine(
                "AccessToken: " +
                loginResponse.AccessToken);
        }
    }
}

public class LoginRequest
{
    public string Id { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string AccessToken { get; set; }
    public int PlayerId { get; set; }
}