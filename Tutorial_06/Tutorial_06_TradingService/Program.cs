using System.Text;
using Tutorial_06_TradingService;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "UEH Trading Processor Service";
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();