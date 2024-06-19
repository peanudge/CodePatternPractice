// See https://aka.ms/new-console-template for more information

var localNowDate = DateTime.Now;
Console.WriteLine(localNowDate.ToString("yyyy-MM-dd HH:mm:ss"));
Console.WriteLine(localNowDate.ToString());
Console.WriteLine(localNowDate.ToBinary());
Console.WriteLine(localNowDate.ToLongDateString());
Console.WriteLine(localNowDate.ToLongTimeString());
Console.WriteLine(localNowDate.ToShortDateString());
Console.WriteLine(localNowDate.ToShortTimeString());

Console.WriteLine(DateTime.MinValue.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ssZ"));
