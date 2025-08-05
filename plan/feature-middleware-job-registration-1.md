---
goal: Enable Consumer Registration of Timed Jobs via Middleware
version: 1.0
date_created: 2025-08-05
last_updated: 2025-08-05
owner: TimedBackgroundJob Team
status: 'Phase 1 Complete'
tags: [feature, middleware, registration, jobs, API]
---

# Introduction

![Status: Planned](https://img.shields.io/badge/status-Planned-blue)

This implementation plan introduces a feature allowing consumers to register timed jobs directly through middleware using a delegate-based API. The goal is to support the following registration pattern:

```csharp
services.AddTimedJob(options =>
{
    options.Interval = TimeSpan.FromMinutes(10);
},
(serviceProvider) => {
    // job logic
});
```

This plan details requirements, implementation steps, alternatives, dependencies, affected files, testing, risks, and related specifications.

## 1. Requirements & Constraints

- **REQ-001**: Consumers must be able to register jobs using a delegate via `AddTimedJob`.
- **REQ-002**: The API must support configuration of job options (e.g., interval).
- **REQ-003**: The job delegate must have access to the DI `IServiceProvider`.
- **CON-001**: Must not break existing attribute-based job registration.
- **CON-002**: Must follow C# best practices and project guidelines.
- **GUD-001**: All public APIs must be documented.
- **PAT-001**: Use standard .NET middleware and DI patterns.

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-001: Design and implement delegate-based job registration API.

| Task      | Description                                                                                                 | Completed | Date       |
|-----------|-------------------------------------------------------------------------------------------------------------|-----------|------------|
| TASK-001  | Update `TimedJobServiceCollectionExtensions.cs` to add overload for delegate-based job registration.         | ✅        | 2025-08-05 |
| TASK-002  | Define a new delegate type for job logic accepting `IServiceProvider`.                                      | ✅        | 2025-08-05 |
| TASK-003  | Update `TimedJobRegistry.cs` to support delegate-based jobs.                                                | ✅        | 2025-08-05 |
| TASK-004  | Update `TimedJobHostedService.cs` to execute delegate-based jobs.                                           | ✅        | 2025-08-05 |
| TASK-005  | Add XML documentation for new APIs.                                                                        | ✅        | 2025-08-05 |

### Implementation Phase 2

- GOAL-002: Integrate, test, and document the new registration method.

| Task      | Description                                                                                                 | Completed | Date       |
|-----------|-------------------------------------------------------------------------------------------------------------|-----------|------------|
| TASK-006  | Add usage examples to `docs/timed-job-usage.md`.                                                            | ✅        | 2025-08-05 |
| TASK-007  | Add unit/integration tests in `TimedBackgroundJob.Tests` for delegate-based jobs.                           | ✅        | 2025-08-05 |
| TASK-008  | Update README.md and example projects to demonstrate new API.                                               | ✅        | 2025-08-05 |
| TASK-009  | Validate backward compatibility with attribute-based jobs.                                                  | ✅        | 2025-08-05 |

## 3. Alternatives

- **ALT-001**: Only support attribute-based job registration. Not chosen due to lack of flexibility for dynamic job logic.
- **ALT-002**: Use custom job interfaces instead of delegates. Not chosen to keep API simple and familiar for middleware scenarios.

## 4. Dependencies

- **DEP-001**: .NET DI and HostedService infrastructure.
- **DEP-002**: Existing TimedBackgroundJob core classes.

## 5. Files

- **FILE-001**: `src/TimedBackgroundJob/TimedJobServiceCollectionExtensions.cs` — Add new overload and delegate type.
- **FILE-002**: `src/TimedBackgroundJob/TimedJobRegistry.cs` — Support delegate-based job registration.
- **FILE-003**: `src/TimedBackgroundJob/TimedJobHostedService.cs` — Execute delegate-based jobs.
- **FILE-004**: `docs/timed-job-usage.md` — Add documentation and usage examples.
- **FILE-005**: `README.md` — Update with new API usage.
- **FILE-006**: `examples/TimedBackgroundJob.Example/Program.cs` — Demonstrate new registration method.
- **FILE-007**: `tests/TimedBackgroundJob.Tests/TimedJobHostedServiceTests.cs` — Add tests for delegate-based jobs.

## 6. Testing

- **TEST-001**: Unit test for delegate-based job execution.
- **TEST-002**: Integration test for job registration via middleware.
- **TEST-003**: Backward compatibility test for attribute-based jobs.
- **TEST-004**: Documentation validation for new API usage.

## 7. Risks & Assumptions

- **RISK-001**: Potential for breaking changes if registry or hosted service logic is not properly isolated.
- **RISK-002**: Delegate-based jobs may introduce new error handling scenarios.
- **ASSUMPTION-001**: Consumers will use DI for job dependencies.
- **ASSUMPTION-002**: Existing attribute-based jobs will remain functional.

## 8. Related Specifications / Further Reading

- [feature-timedjob-attribute-discovery-1.md](../plan/feature-timedjob-attribute-discovery-1.md)
- [.NET Hosted Services documentation](https://learn.microsoft.com/en-us/dotnet/core/extensions/workers)
- [Microsoft Dependency Injection documentation](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
