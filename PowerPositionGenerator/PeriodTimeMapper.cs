namespace PowerPositionGenerator;

using Interfaces;

public class PeriodTimeMapper : IPeriodTimeMapper
{
    public TimeOnly MapPeriodToTime(DateTime date, int period)
    {
        var tz = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
        
        var d = new DateTime(date.Year, date.Month, date.Day, 23, 0, 0, DateTimeKind.Unspecified)
            .AddDays(-1)
            .AddHours(period - 1);

        if (IsLongDay(date))
        {
            d = d.AddHours(-1);
        }
        
        var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(d, tz);

        return TimeOnly.FromDateTime(localDateTime);
    }
    
    private bool IsLongDay(DateTime date)
    {
        return date.IsDaylightSavingTime() && !date.AddDays(1).IsDaylightSavingTime();
    }
}