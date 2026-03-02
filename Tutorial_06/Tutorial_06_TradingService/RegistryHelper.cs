using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Tutorial_06_TradingService
{
    public static class RegistryHelper
    {
        public static TradingConfig GetConfig(ILogger logger)
        {
            var config = new TradingConfig();
            string keyPath = @"Software\TradingService";

            using (RegistryKey? key = Registry.LocalMachine.OpenSubKey(keyPath))
            {
                if (key == null)
                {
                    logger.LogWarning("Registry key không tồn tại. Đang sử dụng cấu hình mặc định.");

                    config.InputFolder = Path.Combine(Path.GetTempPath(), "TradingInput");
                    config.ProcessedFolder = Path.Combine(Path.GetTempPath(), "TradingProcessed");
                    config.IntervalSeconds = 30;
                }
                else
                {
                    try
                    {
                        config.InputFolder = key.GetValue("InputFolder")?.ToString() ?? "";
                        config.ProcessedFolder = key.GetValue("ProcessedFolder")?.ToString() ?? "";
                        config.IntervalSeconds = (int)(key.GetValue("IntervalSeconds") ?? 30);
                        logger.LogInformation("Đã đọc cấu hình từ Registry thành công.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Lỗi khi đọc cấu hình: {ex.Message}");
                    }
                }
            }

            // Tự động tạo thư mục nếu chưa có
            Directory.CreateDirectory(config.InputFolder);
            Directory.CreateDirectory(config.ProcessedFolder);

            return config;
        }
    }
}
