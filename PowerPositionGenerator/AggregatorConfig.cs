namespace PowerPositionGenerator;

public class AggregatorConfig
{
    public TimeSpan AggregationInterval { get; set; } = TimeSpan.FromSeconds(15);
    public string FileFolder { get; set; } = "";
}
