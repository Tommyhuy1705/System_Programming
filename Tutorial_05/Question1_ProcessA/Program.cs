using System.Text;

namespace Question1_ProcessA
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine($"[Process A] Đang chạy với PID: {Environment.ProcessId}");

            int sharedData = 0;

            while (true)
            {
                sharedData++;
                Console.WriteLine($"[Process A] Đang tạo dữ liệu (Produce)... sharedData = {sharedData}");
                Thread.Sleep(2000);
            }
        }
    }
}
