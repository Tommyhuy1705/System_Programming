using System.Diagnostics;
using System;

namespace Tutorial_02
{
    // 1. Struct (Value Type - Stack)
    struct PointStruct
    {
        public int X;
        public int Y;
        public PointStruct(int x, int y) { X = x; Y = y; }
    }

    // 2. Class (Reference Type - Heap)
    class PointClass
    {
        public int X;
        public int Y;
        public PointClass(int x, int y) { X = x; Y = y; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            int size = 1000000;

            // --- STRUCT ---
            GC.Collect();
            long startMem = GC.GetTotalMemory(true);

            PointStruct[] arrayStruct = new PointStruct[size];

            long endMem = GC.GetTotalMemory(true);
            Console.WriteLine($"Struct Array Memory: {(endMem - startMem)} bytes");

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < size; i++) { arrayStruct[i].X = i; }
            sw.Stop();
            Console.WriteLine($"Struct Access Time: {sw.ElapsedMilliseconds} ms");

            // --- CLASS ---
            arrayStruct = null; // Giải phóng
            GC.Collect();
            startMem = GC.GetTotalMemory(true);

            PointClass[] arrayClass = new PointClass[size];
            for (int i = 0; i < size; i++) { arrayClass[i] = new PointClass(0, 0); }

            endMem = GC.GetTotalMemory(true);
            Console.WriteLine($"Class Array Memory: {(endMem - startMem)} bytes");

            sw.Restart();
            for (int i = 0; i < size; i++) { arrayClass[i].X = i; }
            sw.Stop();
            Console.WriteLine($"Class Access Time: {sw.ElapsedMilliseconds} ms");
        }
    }
}
