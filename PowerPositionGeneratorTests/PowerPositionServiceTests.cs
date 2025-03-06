namespace PowerPositionGeneratorTests;

using Microsoft.Extensions.Logging;
using Moq;
using PowerPositionGenerator;
using PowerPositionGenerator.Interfaces;
using Services;

public class PowerPositionServiceTests
{

    private Mock<IPowerService> _powerServiceMock = new (MockBehavior.Strict);
    private Mock<IPowerPositionAggregator> _powerPositionAggregatorMock = new (MockBehavior.Strict);
    private Mock<ICsvFileService> _csvFileServiceMock = new ();
    private Mock<ILogger<PowerPositionService>> _loggerMock = new ();
    
    private PowerPositionService _powerPositionService;

    public PowerPositionServiceTests()
    {
        this._powerPositionService = new PowerPositionService(
            this._powerServiceMock.Object, 
            this._powerPositionAggregatorMock.Object, 
            this._csvFileServiceMock.Object, 
            this._loggerMock.Object);
    }
    
    [Fact]
    public async Task Should_WriteFile_WhenPowerServiceReturnsTrades()
    {
        // Arrange
        var date = new DateTime(2021, 1, 1);
        var powerTrade = PowerTrade.Create(date, 24);
        var powerTrade2 = PowerTrade.Create(date, 24);
        var powerTrades = new List<PowerTrade>{ powerTrade, powerTrade2 };
        
        this._powerServiceMock.Setup(x => x.GetTradesAsync(date))
            .ReturnsAsync(powerTrades);
        
        var aggregatedPowerPositions = new List<PowerPositionAggregate>();
        
        this._powerPositionAggregatorMock.Setup(x => x.Aggregate(powerTrades))
            .Returns(aggregatedPowerPositions);
        
        // Act
        await this._powerPositionService.GeneratePowerPositionFile(date, CancellationToken.None);
        
        // Assert
        this._csvFileServiceMock.Verify(x => x.WritePowerPositionFile(aggregatedPowerPositions, date), Times.Once);
    }
    
    [Fact]
    public async Task Should_RetryAndWriteFile_WhenPowerServiceThrowsException()
    {
        // Arrange
        var date = new DateTime(2021, 1, 1);
        var powerTrade = PowerTrade.Create(date, 24);
        var powerTrade2 = PowerTrade.Create(date, 24);
        var powerTrades = new List<PowerTrade>{ powerTrade, powerTrade2 };
        
        this._powerServiceMock.SetupSequence(x => x.GetTradesAsync(date))
            .ThrowsAsync(new PowerServiceException("Test"))
            .ReturnsAsync(powerTrades);
        
       var aggregatedPowerPositions = new List<PowerPositionAggregate>();
        
        this._powerPositionAggregatorMock.Setup(x => x.Aggregate(powerTrades))
            .Returns(aggregatedPowerPositions);
        
        // Act
        await this._powerPositionService.GeneratePowerPositionFile(date, CancellationToken.None);
        
        // Assert
        this._powerServiceMock.Verify(x => x.GetTradesAsync(date), Times.Exactly(2));
        this._csvFileServiceMock.Verify(x => x.WritePowerPositionFile(aggregatedPowerPositions, date), Times.Once);
    }
}
