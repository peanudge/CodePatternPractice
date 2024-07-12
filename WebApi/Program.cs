using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
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
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }


    // Configure the HTTP request pipeline log
    app.UseSerilogRequestLogging();

    app.UseRouting();

    app.MapControllers();

#pragma warning disable ASP0014 // Suggest using top level route registrations
    _ = app.UseEndpoints(_ => { });
#pragma warning restore ASP0014 // Suggest using top level route registrations

    app.UseSpa(spa =>
    {
        spa.Options.SourcePath = "../spa";
        if (app.Environment.IsDevelopment())
        {
            spa.UseReactDevelopmentServer("start");
        }
    });

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
