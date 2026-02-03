using System;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace Question_04
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("--- QUESTION 4: FILE MONITORING ---");

            string watchPath = Path.Combine(Directory.GetCurrentDirectory(), "WatchFolder");
            Directory.CreateDirectory(watchPath);

            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = watchPath;
                watcher.Filter = "*.txt";
                watcher.Created += OnFileCreated;
                watcher.EnableRaisingEvents = true;

                Console.WriteLine($"Đang theo dõi: {watchPath}");
                Console.WriteLine("Hãy tạo file .txt vào thư mục trên để test.");
                Console.ReadLine();
            }
        }

        private static void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Phát hiện file: {e.Name}");
            ProcessFileSafe(e.FullPath);
        }

        static void ProcessFileSafe(string filePath)
        {
            int retries = 3;
            while (retries > 0)
            {
                try
                {
                    string zipPath = filePath + ".gz";
                    using (FileStream source = new FileStream(filePath, FileMode.Open))
                    using (FileStream target = File.Create(zipPath))
                    using (GZipStream zip = new GZipStream(target, CompressionMode.Compress))
                    {
                        source.CopyTo(zip);
                    }
                    Console.WriteLine($"Đã nén: {zipPath}");
                    return;
                }
                catch (IOException)
                {
                    Thread.Sleep(1000);
                    retries--;
                }
            }
            Console.WriteLine("Lỗi: Không thể truy cập file.");
        }
    }
}