using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Question3_Server
{
    internal class Program
    {
        static async Task Main()
        {
            int port = 8888;
            // 1. Lắng nghe mọi IP tại cổng 8888
            TcpListener server = new TcpListener(IPAddress.Any, port);
            server.Start();
            Console.WriteLine($"[TCP SERVER] Dang lang nghe tai cong {port}...");

            // Chấp nhận 1 client
            using (TcpClient client = await server.AcceptTcpClientAsync())
            using (NetworkStream stream = client.GetStream())
            {
                Console.WriteLine("[TCP SERVER] Da co Client ket noi!");

                // 3. Đọc Request
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string requestMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"[TCP SERVER] Nhan duoc: '{requestMessage}'");

                // Xử lý và trả về Response
                string responseMessage = $"Du lieu sau khi xu ly: {requestMessage.ToUpper()}";
                byte[] responseData = Encoding.UTF8.GetBytes(responseMessage);
                await stream.WriteAsync(responseData, 0, responseData.Length);
                Console.WriteLine("[TCP SERVER] Da gui phan hoi.");
            }
            Console.ReadLine();
        }
    }
}
