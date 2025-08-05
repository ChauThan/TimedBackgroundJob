---
goal: "Implement deterministic versioning for TimedBackgroundJob library with pre-release support"
version: 1.0
date_created: 2025-08-05
last_updated: 2025-08-05
owner: "TimedBackgroundJob Maintainers"
status: 'Planned'
tags: [feature, versioning, release, ci, dotnet]
---

# Introduction

![Status: Planned](https://img.shields.io/badge/status-Planned-blue)

This plan introduces deterministic versioning for the TimedBackgroundJob library. The initial release will be version 0.0.1, with explicit support for pre-release versions. After each release, the version will be incremented and marked as a pre-release according to semantic versioning standards. The process will be automated and integrated into the CI/CD pipeline.

## 1. Requirements & Constraints

- **REQ-001**: The library must use semantic versioning (SemVer) starting at 0.0.1.
- **REQ-002**: Pre-release versions must be supported (e.g., 0.0.2-alpha, 0.0.2-beta).
- **REQ-003**: Versioning must be automated and deterministic.
- **REQ-004**: Version must be updated in all relevant project files and documentation.
- **SEC-001**: No sensitive information should be exposed in versioning or release artifacts.
- **CON-001**: Only .NET-compatible versioning mechanisms may be used.
- **GUD-001**: Follow C# and .NET best practices for versioning and release management.
- **PAT-001**: Use CI/CD pipeline for automated version bump and pre-release tagging.

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-001: Establish initial versioning and pre-release support in the library.

| Task      | Description                                                                                      | Completed | Date       |
|-----------|--------------------------------------------------------------------------------------------------|-----------|------------|
| TASK-001  | Set initial version to 0.0.1 in the build file (e.g., `build.yml`, `Directory.Build.props`) to control project versioning |           |            |
| TASK-002  | Update documentation in `README.md` and `docs/timed-job-usage.md` to reflect version 0.0.1       |           |            |
| TASK-003  | Add version badge to documentation and README                                                    |           |            |
| TASK-004  | Configure .NET project to support pre-release versions (e.g., via `<VersionPrefix>` and `<VersionSuffix>`) |           |            |

### Implementation Phase 2

- GOAL-002: Automate version increment and pre-release marking after each release.

| Task      | Description                                                                                      | Completed | Date       |
|-----------|--------------------------------------------------------------------------------------------------|-----------|------------|
| TASK-005  | Integrate CI/CD workflow to bump version and mark as pre-release (e.g., 0.0.2-alpha) using the build file |           |            |
| TASK-006  | Auto-update version in build file after tagging a release; update release documentation and changelog for each new version |           |            |
| TASK-007  | Validate that pre-release packages are published correctly to NuGet or target registry           |           |            |
| TASK-008  | Ensure version consistency across all project files and documentation                            |           |            |

## 3. Alternatives

- **ALT-001**: Manual versioning via direct file edits (not chosen due to risk of inconsistency and human error).
- **ALT-002**: Use third-party versioning tools (not chosen to maintain native .NET compatibility and simplicity).

## 4. Dependencies

- **DEP-001**: .NET SDK with support for `<VersionPrefix>` and `<VersionSuffix>` in `.csproj`.
- **DEP-002**: CI/CD system (e.g., GitHub Actions, Azure Pipelines) for automation.
- **DEP-003**: NuGet or other package registry for publishing releases.

## 5. Files

- **FILE-001**: `src/TimedBackgroundJob/TimedBackgroundJob.csproj` — main project file for versioning.
- **FILE-002**: `README.md` — documentation for version badge and usage.
- **FILE-003**: `docs/timed-job-usage.md` — usage documentation with version references.
**FILE-004**: `.github/workflows/ci.yml` and `.github/workflows/release.yml` — CI/CD pipeline configuration for NuGet and GitHub Releases.
- **FILE-005**: `CHANGELOG.md` — changelog for release notes.

## 6. Testing

- **TEST-001**: Validate that the version in `.csproj` matches documentation and release notes.
**TEST-002**: Ensure pre-release packages are correctly tagged and published to NuGet and GitHub Releases.
- **TEST-003**: Confirm CI/CD pipeline increments version and marks pre-release as expected.
- **TEST-004**: Automated test to check version badge updates in documentation.

## 7. Risks & Assumptions

- **RISK-001**: CI/CD misconfiguration may result in incorrect versioning or failed releases.
- **RISK-002**: Manual edits outside the automated process may cause version drift.
- **ASSUMPTION-001**: All contributors will use the automated workflow for releases.
- **ASSUMPTION-002**: .NET SDK and CI/CD tools are available and properly configured.

## 8. Related Specifications / Further Reading

- [Semantic Versioning 2.0.0](https://semver.org/)
- [.NET Project Versioning Documentation](https://learn.microsoft.com/en-us/nuget/create-packages/package-versioning)
- [GitHub Actions for .NET](https://github.com/actions/setup-dotnet)
- [NuGet Pre-release Packages](https://learn.microsoft.com/en-us/nuget/create-packages/prerelease-packages)
