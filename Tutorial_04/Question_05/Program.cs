using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace Question_05
{
    class Program
    {
        static byte[] key = Encoding.UTF8.GetBytes("0123456789ABCDEF0123456789ABCDEF");
        static byte[] iv = Encoding.UTF8.GetBytes("0123456789ABCDEF");

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("--- QUESTION 5: SECURITY ---");

            string data = "Hello Systems Programming! " + new string('X', 500);
            string path = "secure.bin";

            SaveSecureData(path, data);
            Console.WriteLine("Đã mã hóa và nén xuống file.");

            string loaded = LoadSecureData(path);
            Console.WriteLine($"Đọc lại dữ liệu: {loaded.Substring(0, 50)}...");

            Console.ReadLine();
        }

        static void SaveSecureData(string path, string data)
        {
            using (FileStream fs = File.Create(path))
            using (GZipStream gzip = new GZipStream(fs, CompressionMode.Compress))
            using (Aes aes = Aes.Create())
            {
                aes.Key = key; aes.IV = iv;
                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                using (CryptoStream cs = new CryptoStream(gzip, encryptor, CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(data);
                }
            }
        }

        static string LoadSecureData(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            using (GZipStream gzip = new GZipStream(fs, CompressionMode.Decompress))
            using (Aes aes = Aes.Create())
            {
                aes.Key = key; aes.IV = iv;
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                using (CryptoStream cs = new CryptoStream(gzip, decryptor, CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}