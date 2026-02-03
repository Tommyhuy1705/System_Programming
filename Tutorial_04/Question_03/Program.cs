using System;
using System.IO;
using System.Threading.Tasks;

namespace Question_03
{
    class Program
    {
        static object _fileLock = new object();
        static string logPath = "log.txt";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("--- QUESTION 3: THREAD-SAFE FILE ACCESS ---");

            if (File.Exists(logPath)) File.Delete(logPath);

            Parallel.For(0, 10, i =>
            {
                LogMessage($"Log entry {i} from Thread {Task.CurrentId}");
            });

            Console.WriteLine("Ghi log hoàn tất. Kiểm tra file log.txt trong thư mục bin/Debug.");
            Console.ReadLine();
        }

        static void LogMessage(string message)
        {
            lock (_fileLock)
            {
                try
                {
                    File.AppendAllText(logPath, $"{DateTime.Now}: {message}{Environment.NewLine}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi: {ex.Message}");
                }
            }
        }
    }
}