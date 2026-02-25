using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Question4_5_Client
{
    public class RpcRequest
    {
        public string Id { get; set; }
        public string Method { get; set; }
        public double[] Params { get; set; } // Dùng mảng double cho dễ demo
    }

    public class RpcResponse
    {
        public string Id { get; set; }
        public double? Result { get; set; }
        public string Error { get; set; }
    }
    class Program
    {
        static async Task Main()
        {
            using (HttpClient client = new HttpClient())
            {
                // 1. Tạo Request gọi hàm MoneyExchange (100 USD)
                var request = new RpcRequest
                {
                    Id = Guid.NewGuid().ToString(),
                    Method = "MoneyExchange",
                    Params = new double[] { 100 }
                };

                // 2. Serialize to JSON
                string jsonPayload = JsonSerializer.Serialize(request);
                Console.WriteLine($"[RPC CLIENT] Gui Request: {jsonPayload}");
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // 3. Gửi tới Server
                var httpResponse = await client.PostAsync("http://localhost:5000/", content);
                string responseJson = await httpResponse.Content.ReadAsStringAsync();

                // 4. Deserialize Response
                var result = JsonSerializer.Deserialize<RpcResponse>(responseJson);
                Console.WriteLine($"[RPC CLIENT] Nhan Response: {responseJson}");

                if (result.Error == null)
                    Console.WriteLine($"\n=> KET QUA: {result.Result} VND");
                else
                    Console.WriteLine($"\n=> LOI: {result.Error}");
            }
            Console.ReadLine();
        }
    }
}
