using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Tutorial_02
{
    class Program
    {
        [DllImport("gdi32.dll", EntryPoint = "AddFontResourceW", SetLastError = true)]
        public static extern int AddFontResource([In][MarshalAs(UnmanagedType.LPWStr)] string lpFileName);

        static void Main()
        {
            // Đường dẫn đến file font
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string fontPath = Path.Combine(baseDir, "LibreBarcode39-Regular.ttf");
            Console.WriteLine($"[Debug] Dang tim file tai: {fontPath}");

            if (File.Exists(fontPath))
            {
                int result = AddFontResource(fontPath);
                if (result > 0)
                    Console.WriteLine("[System] Font installed successfully in memory session.");
                else
                    Console.WriteLine("[Error] Found file but failed to install via GDI32.");
            }
            else
            {
                Console.WriteLine("[Error] KHONG TIM THAY FILE FONT!");
                Console.WriteLine("Hay kiem tra lai buoc 'Copy to Output Directory'.");
            }

            // Mô phỏng in thẻ
            Console.WriteLine("-----------------------------");
            Console.WriteLine("|  UEH MEMBERSHIP CARD      |");
            Console.WriteLine("|  Name: Tran Viet Gia Huy  |");
            Console.WriteLine("|  ID:   31231027056        |");
            Console.WriteLine("|  Level: VIP Member        |");
            Console.WriteLine("-----------------------------");
        }
    }
}