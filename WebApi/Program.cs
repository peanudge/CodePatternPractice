using Serilog;
using Serilog.Events;

// TODO
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Extensions", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    Log.Debug("Starting up");

    // TODO: The builder.Services.AddSerilog() call will redirect all log events through your Serilog pipeline
    builder.Services.AddSerilog();

    builder.Services.AddControllers();

    var app = builder.Build();

    // Configure the HTTP request pipeline log
    app.UseSerilogRequestLogging();
    app.MapGet("/", () => "Hello World!");
    app.Run();

}
catch (Exception ex)
{
    Log.Error(ex, "An error occurred");
}
finally
{
    Log.CloseAndFlush();
}
