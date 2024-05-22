using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var deviceOption = builder.Configuration.GetSection(MyOptions.Device).Get<MyOptions>();

Console.WriteLine(JsonSerializer.Serialize(deviceOption));

var app = builder.Build();

app.MapControllers();

app.Run();

class MyOptions
{
    public const string Device = "Device";
    public string[] Fakes { get; set; } = new string[] { };
    public bool AllFake { get; set; } = false;
}
