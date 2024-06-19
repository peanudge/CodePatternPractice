using Serilog;
using ConsoleApp;
// HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// builder.Services.AddHostedService<Worker>();

// IHost host = builder.Build();

// host.Run();

using var log = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
    .Enrich.FromLogContext()
    .WriteTo.File(
        "logs/log_.txt",
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,
        retainedFileTimeLimit: TimeSpan.FromDays(7))
    .CreateLogger();

Log.Logger = log;

var user = new User();
user.DoSomething();

await Log.CloseAndFlushAsync();

