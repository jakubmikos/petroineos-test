namespace PowerPositionGeneratorTests;

using Moq;
using PowerPositionGenerator;
using PowerPositionGenerator.Interfaces;
using Services;

public class PowerPositionGeneratorTests
{
    private Mock<IPeriodTimeMapper> _periodTimeMapperMock;

    private PowerPositionAggregator _powerPositionGenerator;

    public PowerPositionGeneratorTests()
    {
        this._periodTimeMapperMock = new Mock<IPeriodTimeMapper>(MockBehavior.Strict);

        this._powerPositionGenerator = new PowerPositionAggregator(this._periodTimeMapperMock.Object);
    }

    [Fact]
    public void ShouldAggregatePeriods_AndMapToTimeOfTheDay()
    {
        // Arrange
        var date = new DateTime(2021, 1, 1);
        var powerTrade = PowerTrade.Create(date, 24);
        var powerTrade2 = PowerTrade.Create(date, 24);
        var powerTrades = new List<PowerTrade>{ powerTrade, powerTrade2 };

        this._periodTimeMapperMock.Setup(x => x.MapPeriodToTime(date, It.IsAny<int>()))
            .Returns<DateTime, int>((_, period) => TimeOnly.Parse("23:00:00").AddHours(period - 1));
        
        // Act
        var aggregated = this._powerPositionGenerator.Aggregate(powerTrades).ToList();
        
        // Assert
        for(int i = 0; i < 24; i++)
        {
            var aggregatedPeriod = aggregated[i];
            var expectedTime = TimeOnly.Parse("23:00:00").AddHours(i);
            var expectedAggregatedVolume = powerTrades.SelectMany(x => x.Periods).Where(x => x.Period == (i+1)).Sum(x => x.Volume);
            
            Assert.Equal(aggregatedPeriod.Time, expectedTime);
            Assert.Equal(expectedAggregatedVolume, aggregatedPeriod.Volume);
        }
    }
}
