namespace PowerPositionGenerator.Interfaces;

public interface ICsvFileService
{
    Task WritePowerPositionFile(IEnumerable<PowerPositionAggregate> aggregatedPowerPositions, DateTime date);
}