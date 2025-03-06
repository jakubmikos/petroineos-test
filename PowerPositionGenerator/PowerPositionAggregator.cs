namespace PowerPositionGenerator;

using System.ComponentModel;
using Interfaces;
using Services;

public class PowerPositionAggregator : IPowerPositionAggregator
{
    private readonly IPeriodTimeMapper _periodTimeMapper;
    public PowerPositionAggregator(IPeriodTimeMapper periodTimeMapper)
    {
        this._periodTimeMapper = periodTimeMapper;
    }

    public IEnumerable<PowerPositionAggregate> Aggregate(IEnumerable<PowerTrade> powerTrades)
    {
        var localDate = powerTrades.First().Date;
        
        return powerTrades
            .SelectMany(x => x.Periods)
            .GroupBy(x => x.Period)
            .Select(x => new PowerPositionAggregate
            {
                Time = this._periodTimeMapper.MapPeriodToTime(localDate, x.Key),
                Volume = x.Sum(y => y.Volume),
            });
    }
}