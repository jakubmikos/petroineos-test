namespace PowerPositionGeneratorTests;

using PowerPositionGenerator;

public class PeriodTimeMapperTests
{
    private readonly PeriodTimeMapper _periodTimeMapper = new();
    
    [Theory] 
    [InlineData(1, "23:00", "2025-03-01")]
    [InlineData(2, "00:00", "2025-03-02")]
    [InlineData(3, "01:00", "2025-03-02")]
    [InlineData(4, "02:00", "2025-03-02")]
    [InlineData(5, "03:00", "2025-03-02")]
    [InlineData(6, "04:00", "2025-03-02")]
    [InlineData(7, "05:00", "2025-03-02")]
    [InlineData(8, "06:00", "2025-03-02")]
    [InlineData(9, "07:00", "2025-03-02")]
    [InlineData(10, "08:00", "2025-03-02")]
    [InlineData(11, "09:00", "2025-03-02")]
    [InlineData(12, "10:00", "2025-03-02")]
    [InlineData(13, "11:00", "2025-03-02")]
    [InlineData(14, "12:00", "2025-03-02")]
    [InlineData(15, "13:00", "2025-03-02")]
    [InlineData(16, "14:00", "2025-03-02")]
    [InlineData(17, "15:00", "2025-03-02")]
    [InlineData(18, "16:00", "2025-03-02")]
    [InlineData(19, "17:00", "2025-03-02")]
    [InlineData(20, "18:00", "2025-03-02")]
    [InlineData(21, "19:00", "2025-03-02")]
    [InlineData(22, "20:00", "2025-03-02")]
    [InlineData(23, "21:00", "2025-03-02")]
    [InlineData(24, "22:00", "2025-03-02")]
    // Short day (in 2024 03/31, goes one hour forward from 01:00 to 02:00), 01:00 is skipped
    [InlineData(1, "23:00", "2024-03-31")]
    [InlineData(2, "00:00", "2024-03-31")]
    [InlineData(3, "02:00", "2024-03-31")]
    [InlineData(4, "03:00", "2024-03-31")]
    [InlineData(5, "04:00", "2024-03-31")]
    [InlineData(6, "05:00", "2024-03-31")]
    [InlineData(7, "06:00", "2024-03-31")]
    [InlineData(8, "07:00", "2024-03-31")]
    [InlineData(9, "08:00", "2024-03-31")]
    [InlineData(10, "09:00", "2024-03-31")]
    [InlineData(11, "10:00", "2024-03-31")]
    [InlineData(12, "11:00", "2024-03-31")]
    [InlineData(13, "12:00", "2024-03-31")]
    [InlineData(14, "13:00", "2024-03-31")]
    [InlineData(15, "14:00", "2024-03-31")]
    [InlineData(16, "15:00", "2024-03-31")]
    [InlineData(17, "16:00", "2024-03-31")]
    [InlineData(18, "17:00", "2024-03-31")]
    [InlineData(19, "18:00", "2024-03-31")]
    [InlineData(20, "19:00", "2024-03-31")]
    [InlineData(21, "20:00", "2024-03-31")]
    [InlineData(22, "21:00", "2024-03-31")]
    [InlineData(23, "22:00", "2024-03-31")]
    // Long day (in 2024 10/27, goes one hour back from 02:00 to 01:00)
    [InlineData(1, "23:00", "2024-10-27")]
    [InlineData(2, "00:00", "2024-10-27")]
    [InlineData(3, "01:00", "2024-10-27")]
    [InlineData(4, "01:00", "2024-10-27")] // 01:00 is repeated
    [InlineData(5, "02:00", "2024-10-27")]
    [InlineData(6, "03:00", "2024-10-27")]
    [InlineData(7, "04:00", "2024-10-27")]
    [InlineData(8, "05:00", "2024-10-27")]
    [InlineData(9, "06:00", "2024-10-27")]
    [InlineData(10, "07:00", "2024-10-27")]
    [InlineData(11, "08:00", "2024-10-27")]
    [InlineData(12, "09:00", "2024-10-27")]
    [InlineData(13, "10:00", "2024-10-27")]
    [InlineData(14, "11:00", "2024-10-27")]
    [InlineData(15, "12:00", "2024-10-27")]
    [InlineData(16, "13:00", "2024-10-27")]
    [InlineData(17, "14:00", "2024-10-27")]
    [InlineData(18, "15:00", "2024-10-27")]
    [InlineData(19, "16:00", "2024-10-27")]
    [InlineData(20, "17:00", "2024-10-27")]
    [InlineData(21, "18:00", "2024-10-27")]
    [InlineData(22, "19:00", "2024-10-27")]
    [InlineData(23, "20:00", "2024-10-27")]
    [InlineData(24, "21:00", "2024-10-27")]
    [InlineData(25, "22:00", "2024-10-27")]
    public void MapPeriodToTime_ShouldReturnCorrectTime(int period, string timeString, string dateTime)
    {
        // Arrange
        var date = DateTime.Parse(dateTime);
        var expectedTime = TimeOnly.Parse(timeString);

        // Act
        var result = this._periodTimeMapper.MapPeriodToTime(date, period);

        // Assert
        Assert.Equal(expectedTime, result);
    }

}
