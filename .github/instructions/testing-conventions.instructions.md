---
description: "Use when working in tests/** to keep new tests in the right project and avoid mistakes caused by the mixed target frameworks in AgendaPro.Tests and AgendaPro.UnitTests."
applyTo: "tests/**"
---

# AgendaPro Testing Conventions

- Choose the test project before writing the test.
- Use `tests/AgendaPro.UnitTests` for fast unit tests around Domain and Application behavior. This project currently targets `net10.0` and references `AgendaPro.Application` plus `AgendaPro.Domain`.
- Use `tests/AgendaPro.Tests` for tests that need Infrastructure concerns such as EF Core InMemory or repository-level behavior. This project currently targets `net9.0` and references `AgendaPro.Domain` plus `AgendaPro.Infrastucture`.
- Do not assume the project names are perfectly aligned with their current content. Right now `AgendaPro.UnitTests` contains the more concrete domain/shared tests, while `AgendaPro.Tests` is still relatively sparse.
- Keep new test files near the behavior they cover. Follow the existing folder structure in the chosen test project instead of inventing a new global layout.
- Preserve the repo's current spelling `AgendaPro.Infrastucture` in references and namespaces unless the task is explicitly a rename.

## Framework And Dependency Rules

- Check the target framework in the specific test project before adding packages or references.
- `tests/AgendaPro.UnitTests` targets `net10.0`.
- `tests/AgendaPro.Tests` targets `net9.0`.
- `Directory.Build.props` still defines a default `net8.0` target framework, so always verify the effective framework in the test project's `.csproj` instead of assuming the repo default.
- Avoid copying package versions blindly between the two test projects because their framework versions differ.

## Test Placement

- For `Result`, `Error`, use case orchestration, and other pure business logic, prefer `tests/AgendaPro.UnitTests`.
- For `DbContext`, repository implementations, EF Core InMemory setup, or persistence behavior, prefer `tests/AgendaPro.Tests`.
- If a test would require both infrastructure and application orchestration, stop and choose the smallest useful scope rather than defaulting to a wider integration-style test.

## Execution

- Run the targeted project instead of a broad test sweep when possible.
- Use:
  - `dotnet test tests/AgendaPro.UnitTests/AgendaPro.UnitTests.csproj`
  - `dotnet test tests/AgendaPro.Tests/AgendaPro.Tests.csproj`
- Prefer the root solution `AgendaPro.sln` for restore and build because the nested `src/src.sln` does not include the test projects.

## Common Failure Modes

- Adding a test to the wrong project and then compensating by adding the wrong project references.
- Forgetting that `AgendaPro.Tests` and `AgendaPro.UnitTests` target different frameworks.
- Reading `Directory.Build.props` and assuming every test project inherits `net8.0`.
- Introducing infrastructure-heavy setup into a unit-test slice that should stay fast and isolated.

## Useful Anchors

- [AGENTS.md](../../AGENTS.md)
- [tests/AgendaPro.UnitTests/AgendaPro.UnitTests.csproj](../../tests/AgendaPro.UnitTests/AgendaPro.UnitTests.csproj)
- [tests/AgendaPro.Tests/AgendaPro.Tests.csproj](../../tests/AgendaPro.Tests/AgendaPro.Tests.csproj)
- [Directory.Build.props](../../Directory.Build.props)