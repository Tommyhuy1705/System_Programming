using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Question4_5_Server
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
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/");
            listener.Start();
            Console.WriteLine("[RPC SERVER] Dang lang nghe tai http://localhost:5000/ ...");

            while (true)
            {
                var context = await listener.GetContextAsync();
                _ = HandleRpcRequest(context);
            }
        }

        static async Task HandleRpcRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            using (var reader = new StreamReader(request.InputStream))
            {
                string jsonBody = await reader.ReadToEndAsync();
                var rpcReq = JsonSerializer.Deserialize<RpcRequest>(jsonBody);
                var rpcRes = new RpcResponse { Id = rpcReq?.Id };

                try
                {
                    // Câu 5.2: Hỗ trợ nhiều Method & Error Handling
                    if (rpcReq == null) throw new Exception("Invalid JSON");

                    Console.WriteLine($"[RPC SERVER] Thuc thi ham: {rpcReq.Method}");

                    switch (rpcReq.Method)
                    {
                        case "MoneyExchange":
                            // Giả sử đổi USD sang VND (Tỷ giá 25000)
                            rpcRes.Result = rpcReq.Params[0] * 25000;
                            break;
                        case "Add":
                            rpcRes.Result = rpcReq.Params[0] + rpcReq.Params[1];
                            break;
                        default:
                            // Câu 5.3: Handle Invalid Method
                            rpcRes.Error = $"Method '{rpcReq.Method}' not found.";
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // Câu 5.1: Error handling
                    rpcRes.Error = ex.Message;
                }

                // Gửi kết quả về cho Client
                string responseJson = JsonSerializer.Serialize(rpcRes);
                byte[] buffer = Encoding.UTF8.GetBytes(responseJson);

                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                response.Close();
            }
        }
    }
}
