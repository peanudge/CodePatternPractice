
using Serilog;
namespace ConsoleApp;
public class User
{
    ILogger _logger = Log.ForContext<User>();

    public User()
    {
    }

    public void DoSomething()
    {
        _logger.Information("Doing something");
    }
}
