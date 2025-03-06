namespace PowerPositionGenerator;

using CsvHelper.Configuration.Attributes;

public class PowerPositionAggregate
{
    [Name("Local Time")]
    public TimeOnly Time { get; init; }
    public double Volume { get; init; }
}
