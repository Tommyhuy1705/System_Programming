using System.Text;

namespace Question1_ProcessB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine($"[Process B] Đang chạy với PID: {Environment.ProcessId}");
            Console.WriteLine("[Process B] Muốn tiêu thụ dữ liệu (Consume) từ Process A...");

            int sharedData = 999;

            Console.WriteLine($"[Process B] Dữ liệu 'sharedData' đọc được ở đây là: {sharedData}");
            Console.WriteLine("[Process B] => KẾT LUẬN: B hoàn toàn không nhìn thấy số đếm đang tăng liên tục của A!");
            Console.ReadLine();
        }
    }
}
