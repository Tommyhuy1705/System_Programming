using System;
using System.Threading.Tasks;

namespace Question_02
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("--- QUESTION 2: TASK COORDINATION ---");

            Task t1 = SimulateWork("Task 1", 1000);
            Task t2 = SimulateWork("Task 2", 2000);
            Task t3 = SimulateWork("Task 3", 1500);

            await Task.WhenAll(t1, t2, t3);

            Console.WriteLine("-> TẤT CẢ CÁC TASK ĐÃ HOÀN THÀNH!");
            Console.ReadLine();
        }

        static async Task SimulateWork(string name, int delay)
        {
            Console.WriteLine($"{name} đang chạy...");
            await Task.Delay(delay);
            Console.WriteLine($"{name} xong.");
        }
    }
}