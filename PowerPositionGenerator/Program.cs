using PowerPositionGenerator;
using PowerPositionGenerator.Interfaces;
using Services;

var host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(o => o.ServiceName = "PowerPositionGenerator")
    .ConfigureServices((c, s) => 
    {
            s.AddHostedService<PowerPositionBackgroundService>()
                .AddSingleton<IPowerPositionService, PowerPositionService>()
                .AddSingleton<IPowerService, PowerService>()
                .AddSingleton<IPowerPositionAggregator, PowerPositionAggregator>()
                .AddSingleton<ICsvFileService, CsvFileService>()
                .AddSingleton<IPeriodTimeMapper, PeriodTimeMapper>()
                .Configure<AggregatorConfig>(c.Configuration.GetSection("Aggregator"));
    })
    .Build();

await host.RunAsync();
