---
goal: "Automatic Discovery and Registration of Timed Jobs via TimedJob Attribute"
version: 1.0
date_created: 2025-08-04
last_updated: 2025-08-04
owner: "TimedBackgroundJob Team"
status: 'Planned'
tags: [feature, timed-job, attribute, registration, automation]
---

# Introduction

![Status: Planned](https://img.shields.io/badge/status-Planned-blue)

This implementation plan introduces automatic discovery and registration of timed jobs in the TimedBackgroundJob library. Jobs decorated with the `TimedJob` attribute will be discovered at startup, their options extracted from attribute parameters, and registered automatically with the DI container, eliminating the need for manual registration via `AddTimedJob<TJob>()`.

This update incorporates recommendations for performance mitigation, error handling, extensibility, technical debt tracking, and migration documentation to ensure robust, maintainable, and future-proof delivery.

## 1. Requirements & Constraints

- **REQ-001**: All job classes decorated with the `TimedJob` attribute must be discovered and registered automatically.
- **REQ-002**: Attribute parameters must be mapped to `TimedJobOptions` for each job.
- **REQ-003**: Manual registration via `AddTimedJob<TJob>()` must remain supported for backward compatibility.
- **SEC-001**: Only public, non-abstract classes implementing `ITimedJob` are eligible for automatic registration.
- **CON-001**: Discovery must occur during application startup, before any jobs are executed.
- **GUD-001**: Follow C# 13, .editorconfig, and project coding standards.
- **PAT-001**: Use reflection for type discovery and attribute extraction.
- **PAT-002**: Register discovered jobs and their options using the existing DI pattern.

- **PERF-001**: Restrict reflection-based scanning to relevant assemblies and cache results to mitigate startup performance impact.
- **ERR-001**: Log and surface errors for misconfigured or duplicate jobs during discovery and registration.
- **EXT-001**: Design for future extensibility to support additional job attributes or configuration sources.
- **DOC-001**: Provide migration guidance for users transitioning from manual to automatic registration.

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-001: Implement attribute-based job discovery and registration logic.

| Task      | Description                                                                                                   | Completed | Date       |
|-----------|--------------------------------------------------------------------------------------------------------------|-----------|------------|
| TASK-001  | Define or update the `TimedJob` attribute to support all required options as parameters.                      |           |            |
| TASK-002  | Implement a reflection-based discovery method to scan assemblies for eligible job types with the attribute.   |           |            |
| TASK-003  | Map attribute parameters to `TimedJobOptions` for each discovered job.                                        |           |            |
| TASK-004  | Register discovered jobs and their options in the DI container, mimicking the behavior of `AddTimedJob<TJob>()`. |           |            |
| TASK-005  | Ensure registry and hosted service are registered only once, regardless of discovery or manual registration.  |           |            |

| TASK-006  | Restrict assembly scanning to relevant assemblies and cache discovered job types for performance.              |           |            |
| TASK-007  | Implement error logging and diagnostics for misconfigured or duplicate jobs.                                  |           |            |
| TASK-008  | Design extensibility points for future job attribute/configuration support.                                   |           |            |
| TASK-009  | Track technical debt and deferred improvements in GitHub Issues.                                              |           |            |

### Implementation Phase 2

- GOAL-002: Integrate, validate, and document the new feature.

| Task      | Description                                                                                                   | Completed | Date       |
|-----------|--------------------------------------------------------------------------------------------------------------|-----------|------------|
| TASK-006  | Update documentation to describe attribute-based registration and usage.                                      |           |            |
| TASK-007  | Add XML doc comments and code comments explaining design decisions and edge case handling.                    |           |            |
| TASK-008  | Add unit and integration tests for automatic discovery, registration, and option mapping.                     |           |            |
| TASK-009  | Validate backward compatibility with manual registration.                                                     |           |            |
| TASK-010  | Benchmark startup performance impact and optimize reflection logic if needed.                                 |           |            |

| TASK-011  | Add migration guidance for users moving from manual to automatic registration.                                |           |            |

## 3. Alternatives

- **ALT-001**: Manual registration only (current approach); not chosen due to lack of automation and increased boilerplate.
- **ALT-002**: Use external configuration files for job registration; not chosen due to reduced type safety and discoverability.

## 4. Dependencies

- **DEP-001**: .NET 9.0 or later for reflection and DI features.
- **DEP-002**: Existing TimedBackgroundJob library components (`ITimedJob`, `TimedJobOptions`, etc.).

## 5. Files

- **FILE-001**: `src/TimedBackgroundJob/TimedJobServiceCollectionExtensions.cs` — Extension method updates for automatic registration.
- **FILE-002**: `src/TimedBackgroundJob/TimedJobAttribute.cs` — Attribute definition and parameter mapping.
- **FILE-003**: `src/TimedBackgroundJob/TimedJobOptions.cs` — Ensure compatibility with attribute parameters.
- **FILE-004**: `src/TimedBackgroundJob/TimedJobRegistry.cs` — Registry logic for discovered jobs.
- **FILE-005**: `tests/TimedBackgroundJob.Tests/TimedJobDiscoveryTests.cs` — New/updated tests for discovery and registration.

## 6. Testing

- **TEST-001**: Unit tests for attribute-based job discovery and registration.
- **TEST-002**: Integration tests verifying jobs are executed as per attribute options.
- **TEST-003**: Tests for backward compatibility with manual registration.
- **TEST-004**: Edge case tests (e.g., duplicate jobs, invalid attribute usage).

## 7. Risks & Assumptions

- **RISK-001**: Reflection-based discovery may impact startup performance if many assemblies are loaded.
- **RISK-002**: Incorrect attribute usage may lead to registration failures.
- **ASSUMPTION-001**: All jobs to be registered automatically are decorated with the correct attribute and implement `ITimedJob`.

- **RISK-003**: Failure to log or diagnose errors may hinder maintainability and support.
- **RISK-004**: Lack of extensibility may limit future feature growth.
- **ASSUMPTION-002**: Technical debt will be tracked and addressed in future releases.

## 8. Related Specifications / Further Reading

- [TimedBackgroundJob Documentation](../docs/timed-job-usage.md)
- [.NET Reflection Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/reflection)
- [Microsoft Dependency Injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
- [C# Attributes](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/)
