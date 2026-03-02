using System.Collections.Concurrent;

namespace Tutorial_06_TradingService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private FileSystemWatcher? _watcher;
        private TradingConfig? _config;

        // Dùng ConcurrentDictionary để lock file đang xử lý, chống double processing
        private readonly ConcurrentDictionary<string, bool> _processingFiles = new();

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Trading Service is starting.");
            _config = RegistryHelper.GetConfig(_logger);

            _watcher = new FileSystemWatcher(_config.InputFolder, "*.json");
            _watcher.Created += OnFileCreated;
            _watcher.EnableRaisingEvents = true;

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Trading Service is stopping.");
            _watcher?.Dispose();
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Worker running at: {DateTimeOffset.Now}");
                await Task.Delay((_config?.IntervalSeconds ?? 30) * 1000, stoppingToken);
            }
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            if (!_processingFiles.TryAdd(e.FullPath, true)) return;

            Task.Run(() => ProcessFileSafe(e.FullPath, e.Name));
        }

        private void ProcessFileSafe(string filePath, string? fileName)
        {
            try
            {
                Thread.Sleep(500); // Chờ file copy xong hoàn toàn
                _logger.LogInformation($"Đang xử lý file: {fileName}");

                string content = File.ReadAllText(filePath);
                Thread.Sleep(2000); // Giả lập tốn thời gian xử lý

                if (_config != null && fileName != null)
                {
                    string destPath = Path.Combine(_config.ProcessedFolder, fileName);
                    File.Move(filePath, destPath, true);
                    _logger.LogInformation($"Xử lý xong và di chuyển: {fileName}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi xử lý file {fileName}: {ex.Message}");
            }
            finally
            {
                _processingFiles.TryRemove(filePath, out _);
            }
        }
    }
}