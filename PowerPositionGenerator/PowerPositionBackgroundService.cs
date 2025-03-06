namespace PowerPositionGenerator;

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Interfaces;
using Microsoft.Extensions.Options;
using Services;

public class PowerPositionBackgroundService : BackgroundService
{
    private readonly IPowerPositionService _powerPositionService;
    private readonly AggregatorConfig _aggregatorConfig;
    private readonly ILogger<PowerPositionBackgroundService> _logger;
    
    private PeriodicTimer _timer = null!;
    private CancellationTokenSource _cts = null!;

    public PowerPositionBackgroundService(
        IPowerPositionService powerPositionService,
        ILogger<PowerPositionBackgroundService> logger, 
        IOptions<AggregatorConfig> aggregatorConfig)
    {
        this._logger = logger;
        this._powerPositionService = powerPositionService;
        this._aggregatorConfig = aggregatorConfig.Value;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        this._timer = new PeriodicTimer(this._aggregatorConfig.AggregationInterval);
        this._cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        
        return base.StartAsync(cancellationToken);
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        this._cts.Cancel();
        this._timer.Dispose();
        
        await base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        this._logger.LogInformation("PowerPositionAggregator started at {Time}", DateTimeOffset.Now);
        try
        {
            do
            {
                using var taskCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

                taskCts.CancelAfter(this._aggregatorConfig.AggregationInterval);

                try
                {
                    await this._powerPositionService.GeneratePowerPositionFile(DateTime.Now, taskCts.Token);
                }
                catch (OperationCanceledException) when (taskCts.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
                {
                    // Task was canceled because the interval ran out, not because the service is stopping
                    this._logger.LogError("Failed to aggregate trades. Task canceled because interval time elapsed");
                }
            } while (await this._timer.WaitForNextTickAsync(cancellationToken));
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // Service is stopping, this is expected
        }
        catch (Exception ex)
        {
            this._logger.LogCritical(ex, "Fatal error");
        }
        
        this._logger.LogInformation("PowerPositionAggregator stopped at {Time}", DateTimeOffset.Now);
    }
}