using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lab_MemoryManagement
{
    // Reference Type
    class MyClass
    {
        public int Data;
    }

    // Value Type
    struct MyStruct
    {
        public int Data;
    }

    class Program
    {
        static MyClass globalObject = new MyClass();

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== LAB 3: MEMORY MANAGEMENT & CONCURRENCY ===\n");

            // --- CÂU 1: STACK VS HEAP ---
            Console.WriteLine(">>> Running Question 1...");
            Question1();
            Console.WriteLine("--------------------------------------------------\n");

            // --- CÂU 2: GARBAGE COLLECTION ---
            Console.WriteLine(">>> Running Question 2...");
            Question2();
            Console.WriteLine("--------------------------------------------------\n");

            // --- CÂU 3: MEMORY OPTIMIZATION ---
            Console.WriteLine(">>> Running Question 3...");
            Question3();
            Console.WriteLine("--------------------------------------------------\n");

            // --- CÂU 4: MULTITHREADING ---
            Console.WriteLine(">>> Running Question 4...");
            Question4();
            Console.WriteLine("--------------------------------------------------\n");

            // --- CÂU 5: ASYNC/AWAIT (Phải dùng await) ---
            Console.WriteLine(">>> Running Question 5...");
            await Question5();

            Console.WriteLine("--------------------------------------------------\n");

            Console.WriteLine("=== ALL TASKS COMPLETED. PRESS ENTER TO EXIT. ===");
            Console.ReadLine();
        }

        static void TestScope()
        {
            // Biến này nằm trên Stack, sẽ bị hủy ngay khi hàm TestScope kết thúc (dấu }).
            int scopeVal = 50;

            // Đối tượng MyClass được tạo trên Heap.
            // Biến tham chiếu 'tempObj' nằm trên Stack.
            MyClass tempObj = new MyClass { Data = 60 };

            Console.WriteLine($"In Scope: {scopeVal}, {tempObj.Data}");
        }

        static void Question1()
        {
            int val = 10;
            MyStruct structLocal = new MyStruct { Data = 20 };
            Console.WriteLine($"[Stack] Local Int: {val}");
            Console.WriteLine($"[Stack] Local Struct: {structLocal.Data}");

            // 'refLocal' là biến con trỏ nằm trên Stack, trỏ đến đối tượng MyClass nằm trên Heap
            MyClass refLocal = new MyClass { Data = 30 };
            Console.WriteLine($"[Heap] Local Class Object: {refLocal.Data}");

            Console.WriteLine("-> Bắt đầu gọi hàm TestScope()...");
            TestScope();
            Console.WriteLine("-> Đã thoát khỏi TestScope().");
            Console.WriteLine("   (Lúc này các biến trong TestScope đã bị xóa khỏi Stack,");
            Console.WriteLine("    đối tượng trên Heap trong đó sẽ chờ GC dọn dẹp.)");
        }

        static void Question2()
        {
            Console.WriteLine("\n--- QUESTION 2: GARBAGE COLLECTION ---");

            // Đo bộ nhớ trước
            long before = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory Before: {before} bytes");

            for (int i = 0; i < 5000; i++)
            {
                // Tạo đối tượng tạm
                byte[] buffer = new byte[1024]; // 1KB
            }

            long allocated = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory Allocated (Before GC): {allocated} bytes");

            // Ép chạy GC
            GC.Collect();
            GC.WaitForPendingFinalizers();

            long after = GC.GetTotalMemory(true);
            Console.WriteLine($"Memory After GC: {after} bytes");
        }

        static void Question3()
        {
            Console.WriteLine("\n--- QUESTION 3: MEMORY OPTIMIZATION ---");

            // List<T> tốn bộ nhớ cho việc resize mảng ngầm định.
            List<int> numbers = Enumerable.Range(1, 10000).ToList();
            // LINQ Where tạo ra state machine và delegate (allocations).
            var evenNumbers = numbers.Where(n => n % 2 == 0).ToList();

            // 1. Dùng mảng cố định hoặc Span
            int[] rawNumbers = new int[10000];
            for (int i = 0; i < 10000; i++) rawNumbers[i] = i + 1;

            // 2. Dùng Span<T> để xử lý một vùng nhớ mà không cần copy
            ReadOnlySpan<int> span = rawNumbers.AsSpan();

            // 3. Thay LINQ bằng vòng lặp for đơn giản
            int count = 0;
            foreach (int n in span)
            {
                if (n % 2 == 0) count++;
            }
            Console.WriteLine($"Found {count} even numbers (Optimized).");
        }
        static void Question4()
        {
            Console.WriteLine("\n--- QUESTION 4: MULTITHREADING ---");

            // Cách 1: Manual Thread (Rất tốn kém - Mỗi thread tốn ~1MB Stack)
            for (int i = 0; i < 5; i++)
            {
                Thread t = new Thread(() =>
                {
                    Console.WriteLine($"[Thread] ID: {Thread.CurrentThread.ManagedThreadId}");
                });
                t.Start();
            }

            // Cách 2: ThreadPool (Tái sử dụng thread - Nhẹ nhàng)
            for (int i = 0; i < 5; i++)
            {
                ThreadPool.QueueUserWorkItem((state) =>
                {
                    Console.WriteLine($"[ThreadPool] ID: {Thread.CurrentThread.ManagedThreadId}");
                });
            }

            // Cách 3: Task (Abstraction hiện đại, dùng ThreadPool bên dưới)
            // Task hỗ trợ chờ đợi (Wait), hủy (Cancel), trả về giá trị (Task<T>)
            Parallel.For(0, 5, i =>
            {
                Console.WriteLine($"[Task] ID: {Thread.CurrentThread.ManagedThreadId}");
            });

            Thread.Sleep(1000);
        }
        static async Task Question5()
        {
            Console.WriteLine("\n--- QUESTION 5: ASYNC/AWAIT ---");

            Console.WriteLine("Start doing heavy work...");

            await SimulateWorkAsync();

            Console.WriteLine("Work Done!");
        }

        static async Task SimulateWorkAsync()
        {
            await Task.Delay(1000);
            Console.WriteLine("Finished 1s delay.");
        }
    }
}