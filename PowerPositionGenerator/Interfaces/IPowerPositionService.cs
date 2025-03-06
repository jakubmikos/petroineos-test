namespace PowerPositionGenerator.Interfaces;

public interface IPowerPositionService
{
    Task GeneratePowerPositionFile(DateTime date, CancellationToken cancellationToken);
}