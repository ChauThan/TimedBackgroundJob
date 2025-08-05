---
goal: Update Package Version Based on Tag to Support CI/CD
version: 1.0
date_created: 2025-08-05
last_updated: 2025-08-05
owner: TimedBackgroundJob Maintainers
status: 'Planned'
tags: [infrastructure, ci, cd, versioning, automation]
---

# Introduction

![Status: Planned](https://img.shields.io/badge/status-Planned-blue)

This implementation plan describes the steps required to update the package version in `TimedBackgroundJob.csproj` based on the current git tag, enabling automated versioning for CI/CD workflows. The goal is to ensure that package releases are consistently versioned according to git tags, supporting reliable deployment and distribution.

## 1. Requirements & Constraints

- **REQ-001**: The package version in `TimedBackgroundJob.csproj` must reflect the current git tag during CI/CD builds.
- **REQ-002**: The process must be fully automated and compatible with GitHub Actions.
- **CON-001**: No manual editing of the `.csproj` version for releases.
- **CON-002**: Must not break local development or manual builds.
- **SEC-001**: Only allow version updates from trusted CI/CD pipelines.
- **GUD-001**: Follow semantic versioning for all tags.
- **PAT-001**: Use standard MSBuild properties for version injection.

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-001: Enable dynamic versioning in `TimedBackgroundJob.csproj` using MSBuild properties.

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-001 | Update `TimedBackgroundJob.csproj` to use `$(Version)` property for versioning. | ✅ | 2025-08-05 |
| TASK-002 | Remove hardcoded `<Version>` value if present. | ✅ | 2025-08-05 |
| TASK-003 | Validate that local builds default to a development version if no tag is present. | ✅ | 2025-08-05 |

### Implementation Phase 2

- GOAL-002: Integrate version injection into CI/CD pipeline.

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-004 | Update GitHub Actions workflow to extract git tag and pass as MSBuild `Version` property. | ✅ | 2025-08-05 |
| TASK-005 | Add step to fail CI/CD if no valid tag is present during release builds. | ✅ | 2025-08-05 |
| TASK-006 | Validate that published NuGet packages use the correct version from the tag. | ✅ | 2025-08-05 |

## 3. Alternatives

- **ALT-001**: Manually update the version in `.csproj` for each release (not chosen due to risk of human error and lack of automation).
- **ALT-002**: Use a separate version file (adds complexity and maintenance overhead).

## 4. Dependencies

- **DEP-001**: GitHub Actions workflow file (e.g., `.github/workflows/ci.yml`)
- **DEP-002**: MSBuild and .NET SDK supporting property injection

## 5. Files

- **FILE-001**: `src/TimedBackgroundJob/TimedBackgroundJob.csproj` — main project file for versioning
- **FILE-002**: `.github/workflows/ci.yml` — CI/CD workflow file for version injection

## 6. Testing

- **TEST-001**: Build locally without a tag and verify default version is used.
- **TEST-002**: Build in CI/CD with a tag and verify correct version is injected.
- **TEST-003**: Publish package and confirm NuGet version matches git tag.

## 7. Risks & Assumptions

- **RISK-001**: CI/CD pipeline may fail if tag extraction logic is incorrect.
- **RISK-002**: Local builds may not reflect intended version if not handled properly.
- **ASSUMPTION-001**: Git tags follow semantic versioning and are present for all releases.

## 8. Related Specifications / Further Reading

- [MSBuild: How to set version from command line](https://docs.microsoft.com/en-us/nuget/create-packages/creating-a-package#automatically-populating-package-version)
- [GitHub Actions: Using git tags in workflows](https://docs.github.com/en/actions/using-workflows/workflow-syntax-for-github-actions#example-using-tags)
- [NuGet: Versioning best practices](https://docs.microsoft.com/en-us/nuget/reference/package-versioning)
