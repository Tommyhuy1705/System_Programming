using System;
using System.Threading;
using System.Threading.Tasks;

namespace Question_01
{
    class Program
    {
        static int sharedCounter = 0;
        static object _lock = new object();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("--- QUESTION 1: RACE CONDITION ---");

            // 1. Race Condition
            sharedCounter = 0;
            Parallel.For(0, 100000, i => { sharedCounter++; });
            Console.WriteLine($"Kết quả lỗi (Không Lock): {sharedCounter}");

            // 2. Lock
            sharedCounter = 0;
            Parallel.For(0, 100000, i => { lock (_lock) { sharedCounter++; } });
            Console.WriteLine($"Kết quả đúng (Dùng Lock): {sharedCounter}");

            // 3. Interlocked
            sharedCounter = 0;
            Parallel.For(0, 100000, i => { Interlocked.Increment(ref sharedCounter); });
            Console.WriteLine($"Kết quả tối ưu (Interlocked): {sharedCounter}");

            Console.ReadLine();
        }
    }
}