using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace Question2_Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Server: Waiting for client...");

            // Tạo Server Pipe
            using (var server = new NamedPipeServerStream("PipesOfHuy"))
            {
                await server.WaitForConnectionAsync();
                Console.WriteLine("Server: Client connected!");

                using (var reader = new StreamReader(server))
                using (var writer = new StreamWriter(server) { AutoFlush = true })
                {
                    // 3. Đọc tin nhắn từ Client
                    string message = await reader.ReadLineAsync();
                    Console.WriteLine($"Server received: {message}");

                    // Gửi phản hồi
                    await writer.WriteLineAsync($"Hello Client, I received '{message}' at {DateTime.Now}");
                }
            }
            Console.ReadLine();
        }
    }
}
