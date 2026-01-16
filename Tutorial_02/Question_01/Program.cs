using System;
using System.Security.Claims;

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
            // --- STRUCT ---
            Console.WriteLine("--- STRUCT ---");
            PointStruct s1 = new PointStruct(10, 10);
            PointStruct s2 = s1;
            s2.X = 999;
            Console.WriteLine($"s1.X: {s1.X}");
            Console.WriteLine($"s2.X: {s2.X}");
            // s2 là bản sao độc lập, sửa s2 không ảnh hưởng s1

            // --- CLASS ---
            Console.WriteLine("\n--- CLASS ---");
            PointClass c1 = new PointClass(10, 10);
            PointClass c2 = c1;
            c2.X = 999;
            Console.WriteLine($"c1.X: {c1.X}"); // Bị đổi thành 999
            Console.WriteLine($"c2.X: {c2.X}"); // Là 999
            // c1 và c2 cùng trỏ vào một vùng nhớ trên Heap

            Console.ReadLine();
        }
    }
}
//--- STRUCT ---
//s1.X: 10
//s2.X: 999

//--- CLASS ---
//c1.X: 999
//c2.X: 999