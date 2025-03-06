namespace PowerPositionGenerator.Interfaces;

public interface IPeriodTimeMapper
{
    TimeOnly MapPeriodToTime(DateTime date, int period);
}