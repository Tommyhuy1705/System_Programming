using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace Question2_Client
{
    internal class Program
    {
        static async Task Main()
        {
            Console.WriteLine("[PIPE CLIENT] Dang tim kiem Server...");

            using (var client = new NamedPipeClientStream(".", "UehIpcPipe", PipeDirection.InOut))
            {
                await client.ConnectAsync(5000);
                Console.WriteLine("[PIPE CLIENT] Da ket noi voi Server!");

                using (var writer = new StreamWriter(client) { AutoFlush = true })
                using (var reader = new StreamReader(client))
                {
                    // Gửi tin nhắn
                    Console.WriteLine("[PIPE CLIENT] Dang gui tin nhan...");
                    await writer.WriteLineAsync("Xin chao, toi la Process Client day!");

                    // 4. Đọc phản hồi
                    string serverResponse = await reader.ReadLineAsync();
                    Console.WriteLine($"[PIPE CLIENT] Server phan hoi: '{serverResponse}'");
                }
            }
            Console.ReadLine();
        }
    }
}
