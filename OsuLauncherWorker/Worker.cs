using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace OsuLauncherWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly string _processName = "osu!.exe";

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            CheckForOsuProcess();
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }

    private void CheckForOsuProcess()
    {
        Process[] processes = Process.GetProcessesByName(_processName);

        if (processes.Length > 0)
        {
            _logger.LogInformation($"{_processName} is running.");
        }
    }
}