using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // 7778번 포트에서 UDP 데이터 수신
        using UdpClient receiver = new(7778);

        Console.WriteLine("UDP 수신 대기");

        // 데이터그램 하나가 올 때까지 기다림
        UdpReceiveResult result =
            await receiver.ReceiveAsync();

        // 받은 바이트를 문자열로 변환
        string message =
            Encoding.UTF8.GetString(result.Buffer);

        Console.WriteLine($"수신: {message}");
    }
}