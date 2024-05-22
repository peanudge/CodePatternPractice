PrepareTimeoutTimer prepareTimeoutTimer = new();
prepareTimeoutTimer.PrepareTimeout(1);
prepareTimeoutTimer.PrepareTimeout(2);
await Task.Delay(1000);
prepareTimeoutTimer.ResolveTimeout(1);
prepareTimeoutTimer.PrepareTimeout(2);
await Task.Delay(3000);

// AirflowMonitoringService => Delegate AirflowPreparationTimeoutTimer => Resolve, Prepare

public class PrepareTimeoutTimer
{
    private Dictionary<long, DateTimeOffset> _prepareTimes = new();
    private TimeSpan _timeoutMilliseconds = TimeSpan.FromMilliseconds(2000);

    private Timer _timeoutTimer;

    public PrepareTimeoutTimer(
        Action onTimeout = null,
        Action onResolved = null,
        TimeSpan? timeoutMilliseconds = null
    )
    {
        _timeoutTimer = new Timer(new TimerCallback(_ =>
        {
            CheckPrepareTimeout();
        }));

        _timeoutTimer.Change(
            dueTime: 0,
            period: 100
        );
    }

    private void CheckPrepareTimeout()
    {
        var now = DateTimeOffset.UtcNow;
        Console.WriteLine($"Tick {now}");
        foreach (var (id, preparedTime) in _prepareTimes)
        {
            if (now >= preparedTime.Add(_timeoutMilliseconds))
            {
                // Timeout
                Console.WriteLine($"Id:{id} => Timeout!!");
                _prepareTimes.Remove(id);
            }
        }
    }

    public void PrepareTimeout(long id)
    {
        Console.WriteLine("Prepare => " + id);
        _prepareTimes[id] = DateTimeOffset.UtcNow;
    }

    public void ResolveTimeout(long id)
    {
        Console.WriteLine("Prepare => " + id);
        _prepareTimes.Remove(id);
    }
}
