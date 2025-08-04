---
goal: Create an Example Project to Demonstrate Usage of TimedBackgroundJob Library
version: 1.0
date_created: 2025-08-04
last_updated: 2025-08-04
owner: ChauThan
status: 'Planned'
tags: [feature, demo, documentation, example, usage]
---

# Introduction

![Status: Planned](https://img.shields.io/badge/status-Planned-blue)

This implementation plan defines the steps to create a fully functional example project that demonstrates how to use the TimedBackgroundJob library. The goal is to provide users with a clear, executable reference for integrating and utilizing the library in their own applications.


## 1. Requirements & Constraints

- **REQ-001**: The example project must use the TimedBackgroundJob library from the local `src/TimedBackgroundJob` directory.
- **REQ-002**: The example must include at least two custom jobs implementing `ITimedJob`.
- **REQ-003**: The example must demonstrate configuration and registration of timed jobs.
- **REQ-004**: The project must build and run successfully using `dotnet`.
- **REQ-005**: The example must include documentation in `docs/` describing usage and setup.
- **CON-001**: All code must follow C# guidelines as specified in `.github/instructions/csharp.instructions.md`.
- **GUD-001**: Use clear, self-explanatory naming conventions for all classes and files.
- **PAT-001**: Example must use dependency injection for job registration.

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-001: Scaffold Example Project Structure

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-001 | Create `examples/TimedBackgroundJob.Example/` directory and project files |  |  |
| TASK-002 | Add reference to `src/TimedBackgroundJob` library in example project |  |  |
| TASK-003 | Create initial `Program.cs` with basic host setup |  |  |

### Implementation Phase 2

- GOAL-002: Implement Example Jobs and Integration

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-004 | Implement at least two sample job classes (e.g., `HelloWorldJob.cs`, `TimeLoggerJob.cs`) in example project |  |  |
| TASK-005 | Register both sample jobs using DI and configure job options in `Program.cs` |  |  |
| TASK-006 | Add usage documentation to `docs/timed-job-usage.md` including both jobs |  |  |

### Implementation Phase 3

- GOAL-003: Validate, Test, and Document Example Project

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-007 | Build and run the example project to verify correct job execution |  |  |
| TASK-008 | Add README to `examples/TimedBackgroundJob.Example/` with setup instructions |  |  |
| TASK-009 | Review code for compliance with C# guidelines and best practices |  |  |

## 3. Alternatives

- **ALT-001**: Use a console application instead of a hosted service for the example. Not chosen to better reflect real-world usage.
- **ALT-002**: Provide only code snippets in documentation. Not chosen to ensure a complete, runnable demo project.

## 4. Dependencies

- **DEP-001**: .NET SDK (compatible version for TimedBackgroundJob)
- **DEP-002**: TimedBackgroundJob library (local project reference)
- **DEP-003**: Microsoft.Extensions.Hosting (for host setup)
- **DEP-004**: Any additional NuGet packages required for DI or configuration

## 5. Files

- **FILE-001**: `examples/TimedBackgroundJob.Example/Program.cs` — Main entry point and host setup
- **FILE-002**: `examples/TimedBackgroundJob.Example/HelloWorldJob.cs` — Sample job implementation
- **FILE-003**: `examples/TimedBackgroundJob.Example/TimeLoggerJob.cs` — Second sample job implementation
- **FILE-004**: `examples/TimedBackgroundJob.Example/TimedBackgroundJob.Example.csproj` — Project file
- **FILE-004**: `docs/timed-job-usage.md` — Usage documentation
- **FILE-005**: `examples/TimedBackgroundJob.Example/README.md` — Example project instructions

## 6. Testing

- **TEST-001**: Build test — Ensure example project builds without errors
- **TEST-002**: Run test — Verify that both sample jobs execute as expected
- **TEST-003**: Integration test — Confirm job registration and configuration works for both jobs
- **TEST-004**: Documentation test — Validate that usage instructions are clear and complete for both jobs

## 7. Risks & Assumptions

- **RISK-001**: Example project may not reflect all advanced features of the library
- **RISK-002**: Changes in the main library may break the example if not kept in sync
- **ASSUMPTION-001**: Users have .NET SDK installed and available

## 8. Related Specifications / Further Reading

- [TimedBackgroundJob Library Documentation](../docs/timed-job-usage.md)
- [C# Coding Guidelines](../../.github/instructions/csharp.instructions.md)
- [.NET Generic Host Documentation](https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host)
- [Dependency Injection in .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)

---
