namespace PowerPositionGenerator;

using System.Globalization;
using CsvHelper;
using Interfaces;
using Microsoft.Extensions.Options;

public class CsvFileService : ICsvFileService
{
    private readonly AggregatorConfig _aggregatorConfig;
    public CsvFileService(IOptions<AggregatorConfig> aggregatorConfig)
    {
        this._aggregatorConfig = aggregatorConfig.Value;
    }
    
    public async Task WritePowerPositionFile(IEnumerable<PowerPositionAggregate> aggregatedPowerPositions, DateTime date)
    {
        var filePath = Path.Combine(this._aggregatorConfig.FileFolder, $"PowerPositions_{date:yyyyMMdd-HHmm}.csv");
        
        await using var writer = new StreamWriter(filePath);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        
        await csv.WriteRecordsAsync(aggregatedPowerPositions);
    }

}
