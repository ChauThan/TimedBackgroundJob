---
goal: Create a Timed Background Job Library for .NET
version: 1.0
date_created: 2025-08-04
last_updated: 2025-08-04
owner: ChauThan
status: 'Completed'
tags: [feature, library, background-job, .NET, architecture]
---

# Introduction

![Status: Completed](https://img.shields.io/badge/status-Completed-brightgreen)

This implementation plan describes the creation of a .NET library that enables users to register and run timed background jobs via dependency injection. The library will provide an extension method for `IServiceCollection` to register jobs with configurable intervals, e.g.:
```csharp
services.AddTimedJob<CustomJob>(options => options.Interval = EveryHour);
```
The goal is to deliver a robust, testable, and extensible background job system for .NET applications.

## 1. Requirements & Constraints

- **REQ-001**: Provide an extension method `AddTimedJob<TJob>` for `IServiceCollection`
- **REQ-002**: Support job interval configuration via options delegate
- **REQ-003**: Jobs must run in the background at specified intervals
- **REQ-004**: Jobs must be DI-resolvable and support scoped, transient, and singleton lifetimes
- **REQ-005**: Library must be compatible with .NET 6.0+ and .NET 9.0
- **SEC-001**: Ensure thread safety and prevent overlapping job executions
- **CON-001**: No external dependencies except Microsoft.Extensions.*
- **GUD-001**: Follow C# coding guidelines from `.github/instructions/csharp.instructions.md`
- **PAT-001**: Use HostedService pattern for background execution

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-001: Define core interfaces, options, and extension methods

| Task      | Description                                                                                  | Completed | Date       |
|-----------|----------------------------------------------------------------------------------------------|-----------|------------|
| TASK-001  | Create `ITimedJob` interface in `src/TimedBackgroundJob/ITimedJob.cs`                        | ✅ | 2025-08-04 |
| TASK-002  | Define `TimedJobOptions` class in `src/TimedBackgroundJob/TimedJobOptions.cs`                | ✅ | 2025-08-04 |
| TASK-003  | Implement `AddTimedJob<TJob>` extension method in `src/TimedBackgroundJob/TimedJobServiceCollectionExtensions.cs` | ✅ | 2025-08-04 |

### Implementation Phase 2

- GOAL-002: Implement job runner, hosted service, and registration logic

| Task      | Description                                                                                  | Completed | Date       |
|-----------|----------------------------------------------------------------------------------------------|-----------|------------|
| TASK-004  | Implement `TimedJobHostedService` in `src/TimedBackgroundJob/TimedJobHostedService.cs`       | ✅ | 2025-08-04 |
| TASK-005  | Implement job runner logic to execute jobs at configured intervals                           | ✅ | 2025-08-04 |
| TASK-006  | Add support for multiple jobs and ensure thread safety                                       | ✅ | 2025-08-04 |

### Implementation Phase 3

- GOAL-003: Add documentation, samples, and tests

| Task      | Description                                                                                  | Completed | Date       |
|-----------|----------------------------------------------------------------------------------------------|-----------|------------|
| TASK-007  | Create usage documentation in `docs/timed-job-usage.md`                                      | ✅ | 2025-08-04 |
| TASK-008  | Add sample job implementation in `samples/CustomJob.cs`                                      | ✅ | 2025-08-04 |
| TASK-009  | Implement unit and integration tests in `tests/TimedBackgroundJob.Tests/`                    | ✅ | 2025-08-04 |

## 3. Alternatives

- **ALT-001**: Use Quartz.NET for job scheduling (not chosen due to external dependency constraint)
- **ALT-002**: Use Timer directly in user code (not chosen for lack of DI integration and testability)

## 4. Dependencies

- **DEP-001**: Microsoft.Extensions.DependencyInjection
- **DEP-002**: Microsoft.Extensions.Hosting

## 5. Files

- **FILE-001**: `src/TimedBackgroundJob/ITimedJob.cs` - Interface for timed jobs
- **FILE-002**: `src/TimedBackgroundJob/TimedJobOptions.cs` - Options for job configuration
- **FILE-003**: `src/TimedBackgroundJob/TimedJobServiceCollectionExtensions.cs` - Extension method for registration
- **FILE-004**: `src/TimedBackgroundJob/TimedJobHostedService.cs` - Hosted service for job execution
- **FILE-005**: `docs/timed-job-usage.md` - Documentation
- **FILE-006**: `samples/CustomJob.cs` - Sample job implementation
- **FILE-007**: `tests/TimedBackgroundJob.Tests/` - Test project

## 6. Testing

- **TEST-001**: Unit test for job registration and options parsing
- **TEST-002**: Integration test for job execution at correct intervals
- **TEST-003**: Thread safety and non-overlapping execution test
- **TEST-004**: DI lifetime compatibility test

## 7. Risks & Assumptions

- **RISK-001**: Overlapping job executions if not properly synchronized
- **RISK-002**: Misconfiguration of intervals leading to performance issues
- **ASSUMPTION-001**: Consumers will implement jobs using the provided interface

## 8. Related Specifications / Further Reading

- [Microsoft Docs: Background tasks with hosted services](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services)
- [.github/instructions/csharp.instructions.md](../../.github/instructions/csharp.instructions.md)
- [Quartz.NET documentation](https://www.quartz-scheduler.net/documentation/)
