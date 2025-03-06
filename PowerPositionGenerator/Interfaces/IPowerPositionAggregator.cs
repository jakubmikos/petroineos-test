namespace PowerPositionGenerator.Interfaces;

using Services;

public interface IPowerPositionAggregator
{
    IEnumerable<PowerPositionAggregate> Aggregate(IEnumerable<PowerTrade> powerTrades);
}