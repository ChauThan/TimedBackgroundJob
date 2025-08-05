# TimedBackgroundJob

![Status: Planned](https://img.shields.io/badge/status-Planned-blue)
![NuGet Version](https://img.shields.io/badge/version-0.0.1--alpha-blue)

<!-- CI/CD Status Badges -->
![CI](https://github.com/ChauThan/TimedBackgroundJob/actions/workflows/ci.yml/badge.svg)
![Release](https://github.com/ChauThan/TimedBackgroundJob/actions/workflows/release.yml/badge.svg)

> **CI/CD:** Automated build, test, and NuGet publishing via GitHub Actions. See `.github/workflows/ci.yml` and `.github/workflows/release.yml`.

A robust .NET library for scheduling and running background jobs at timed intervals. Designed for reliability, extensibility, and ease of integration into ASP.NET Core applications.

## Features

- Attribute-based job scheduling
- Hosted service for background execution
- Overlap prevention and error handling
- Extensible job registry
- Example and test projects included

## Architecture Overview

TimedBackgroundJob uses a registry to discover and manage jobs marked with `[TimedJob]` attributes. Jobs are executed by a hosted service, with options for concurrency control and error management.

## Current Version

The current version of TimedBackgroundJob is **0.0.1-alpha**. Versioning is managed via the build file (`Directory.Build.props`) and CI/CD automation. Pre-release versions are supported and published automatically after each release tag.

## Setup Instructions

### Prerequisites
- .NET 9.0 SDK or later

### Build Steps

```pwsh
# Clone the repository
$ git clone https://github.com/ChauThan/TimedBackgroundJob.git
cd TimedBackgroundJob

# Restore dependencies
$ dotnet restore

# Build the solution
$ dotnet build

# Run tests
$ dotnet test
```

## Usage Examples
### Delegate-Based Job Registration

You can register a timed job using a delegate for inline logic and DI access:

```csharp
services.AddTimedJob(options =>
{
    options.Interval = TimeSpan.FromMinutes(10);
    options.Name = "DelegateJob";
},
(serviceProvider) =>
{
    var logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger("DelegateJob");
    logger?.LogInformation($"Delegate job executed at {DateTime.Now:HH:mm:ss}");
    return Task.CompletedTask;
});
```

### Library Integration

Add the NuGet package or reference the project, then register jobs in your ASP.NET Core app:

```csharp
// Register the job in Startup.cs or Program.cs
services.AddTimedJob<MyJob>(options =>
{
    options.Interval = TimeSpan.FromMinutes(5); // Runs every 5 minutes
});
```

Define a job:

```csharp
using TimedBackgroundJob;

public class MyJob : ITimedJob
{
    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        // Job logic here
        Console.WriteLine($"MyJob executed at {DateTime.Now:HH:mm:ss}");
        return Task.CompletedTask;
    }
}
```

### Example Project

See `examples/TimedBackgroundJob.Example/` for sample jobs and usage patterns. Run the example:

```pwsh
cd examples/TimedBackgroundJob.Example
$ dotnet run
```

## Contribution Guidelines

Contributions are welcome! Please review the [code of conduct](https://github.com/ChauThan/TimedBackgroundJob/blob/main/CODE_OF_CONDUCT.md) and submit pull requests with clear descriptions.

- Fork the repository
- Create a feature branch
- Submit a pull request

## License

This project is licensed under the terms of the [LICENSE](./LICENSE) file.

## Documentation & Further Reading

- [Usage Guide](./docs/timed-job-usage.md)
- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [GitHub Markdown Guide](https://guides.github.com/features/mastering-markdown/)

---

For questions or support, please open an issue on GitHub.
