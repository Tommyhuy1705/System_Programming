using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Question3_Client
{
    internal class Program
    {
        static async Task Main()
        {
            Console.WriteLine("[TCP CLIENT] Dang ket noi...");
            using (TcpClient client = new TcpClient())
            {
                // 2. Kết nối tới localhost cổng 8888
                await client.ConnectAsync("127.0.0.1", 8888);
                Console.WriteLine("[TCP CLIENT] Ket noi thanh cong!");

                using (NetworkStream stream = client.GetStream())
                {
                    // Gửi Request
                    string message = "hello the gioi tcp";
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(data, 0, data.Length);
                    Console.WriteLine($"[TCP CLIENT] Da gui: '{message}'");

                    // Đọc Response
                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"[TCP CLIENT] Server tra ve: '{response}'");
                }
            }
            Console.ReadLine();
        }
    }
}
