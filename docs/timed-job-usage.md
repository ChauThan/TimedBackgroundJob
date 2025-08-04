# Timed Background Job Library Usage
## Example: Registering HelloWorldJob and TimeLoggerJob

In your example project, register both jobs in `Program.cs`:

```csharp
services.AddTimedJob<HelloWorldJob>(options =>
{
    options.Interval = TimeSpan.FromSeconds(10);
    options.Name = "HelloWorldJob";
});
services.AddTimedJob<TimeLoggerJob>(options =>
{
    options.Interval = TimeSpan.FromSeconds(15);
    options.Name = "TimeLoggerJob";
});
```

## Example Job Implementations

`HelloWorldJob.cs`:
```csharp
public class HelloWorldJob : ITimedJob
{
    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine($"Hello, World! The time is {DateTime.Now:HH:mm:ss}");
        return Task.CompletedTask;
    }
}
```

`TimeLoggerJob.cs`:
```csharp
public class TimeLoggerJob : ITimedJob
{
    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine($"Current time logged: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        return Task.CompletedTask;
    }
}
```

## Registering a Timed Job

Add the following to your `Startup.cs` or wherever you configure services:

```csharp
services.AddTimedJob<CustomJob>(options =>
{
    options.Interval = TimeSpan.FromHours(1);
    options.PreventOverlap = true;
});
```

## Implementing a Custom Job

Create a class that implements `ITimedJob`:

```csharp
using TimedBackgroundJob;
using System.Threading;
using System.Threading.Tasks;

public class CustomJob : ITimedJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        // Your job logic here
        await Task.Delay(1000, cancellationToken);
    }
}
```

## Multiple Jobs

You can register multiple jobs by calling `AddTimedJob<TJob>` for each job type:

```csharp
services.AddTimedJob<CustomJob>(options =>
{
    options.Interval = TimeSpan.FromMinutes(10);
});
services.AddTimedJob<AnotherJob>(options =>
{
    options.Interval = TimeSpan.FromHours(1);
    options.PreventOverlap = true;
});
```

All registered jobs are managed by the library and executed according to their configured intervals. The registry automatically collects all jobs via dependency injection, so each job runs independently and concurrently as needed.

## Options
- `Interval`: Time between job executions
- `PreventOverlap`: Prevents concurrent executions of the same job

## Notes
- Jobs are resolved via DI and can use scoped, transient, or singleton dependencies.
- All jobs run in the background using hosted services.
