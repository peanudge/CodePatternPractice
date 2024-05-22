// See https://aka.ms/new-console-template for more information

var utcNowDate = DateTime.UtcNow;
Console.WriteLine(utcNowDate.Kind);
Console.WriteLine(utcNowDate);
var localNowDate = DateTime.Now;
Console.WriteLine(localNowDate.Kind);
Console.WriteLine(localNowDate);
