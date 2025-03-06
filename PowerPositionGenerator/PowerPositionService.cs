namespace PowerPositionGenerator;

using Interfaces;
using Services;

public class PowerPositionService : IPowerPositionService
{
    private readonly IPowerService _powerService;
    private readonly IPowerPositionAggregator _powerPositionAggregator;
    private readonly ICsvFileService _csvFileService;
    private readonly ILogger<PowerPositionService> _logger;



    public PowerPositionService(IPowerService powerService, IPowerPositionAggregator powerPositionAggregator, ICsvFileService csvFileService, ILogger<PowerPositionService> logger)
    {
        this._powerService = powerService;
        this._powerPositionAggregator = powerPositionAggregator;
        this._csvFileService = csvFileService;
        this._logger = logger;
    }
    
    public async Task GeneratePowerPositionFile(DateTime date, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                this._logger.LogInformation("Aggregating trades for {Date} [{Timestamp}]", date, DateTimeOffset.Now);

                var trades = await this._powerService.GetTradesAsync(date);

                var aggregatedPowerPositions = this._powerPositionAggregator.Aggregate(trades);

                await this._csvFileService.WritePowerPositionFile(aggregatedPowerPositions, date);

                this._logger.LogInformation("Trades for {Date} aggregated", date);

                return;
            }
            catch (PowerServiceException pse)
            {
                // as skipping the processing of the trades for the day is not acceptable, we retry as long as we can (until the aggregation interval runs out)
                this._logger.LogWarning(pse, "Fetching trades failed. Retrying in 1 second");
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }

        cancellationToken.ThrowIfCancellationRequested();
    }

}