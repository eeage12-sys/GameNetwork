using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        using UdpClient receiver = new(7778);

        int lastSequence = -1;

        Console.WriteLine("UDP 상태 수신 대기");

        for (int i = 0; i < 3; i++)
        {
            UdpReceiveResult result =
                await receiver.ReceiveAsync();

            string message =
                Encoding.UTF8.GetString(result.Buffer);

            Console.WriteLine($"수신: {message}");

            string[] parts =
                message.Split('|');

            if (parts.Length != 4 ||
                parts[0] != "POSITION")
            {
                Console.WriteLine("잘못된 메시지 형식");
                continue;
            }

            int sequence = int.Parse(parts[1]);
            int x = int.Parse(parts[2]);
            int y = int.Parse(parts[3]);

            if (sequence > lastSequence)
            {
                if (lastSequence >= 0 &&
                    sequence > lastSequence + 1)
                {
                    Console.WriteLine(
                        $"누락 가능성: sequence {lastSequence + 1} 누락");
                }

                Console.WriteLine(
                    $"최신 위치 반영: sequence={sequence}, x={x}, y={y}");

                lastSequence = sequence;
            }
            else
            {
                Console.WriteLine(
                    $"오래된 위치 무시: sequence={sequence}");
            }
        }
    }
}