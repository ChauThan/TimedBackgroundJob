# TimedBackgroundJob.Example

This example demonstrates how to use the TimedBackgroundJob library with two sample jobs: `HelloWorldJob` and `TimeLoggerJob`.

## Setup

1. Ensure you have the .NET SDK installed.
2. Build and run the example project:
   ```pwsh
   dotnet run --project examples/TimedBackgroundJob.Example/TimedBackgroundJob.Example.csproj
   ```

## Jobs

- **HelloWorldJob**: Prints a hello message with the current time every 10 seconds.
- **TimeLoggerJob**: Logs the current time every 15 seconds.

## How It Works

- Jobs are registered using dependency injection in `Program.cs`.
- The TimedBackgroundJob library manages job execution intervals and prevents overlap if configured.

For more details, see [docs/timed-job-usage.md](../../docs/timed-job-usage.md).
