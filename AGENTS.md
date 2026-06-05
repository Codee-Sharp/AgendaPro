# AgendaPro Agent Guide

## Start Here

- Prefer the root solution `AgendaPro.sln` for restore and build. The nested `src/src.sln` does not include the test projects.
- Use these commands first:
  - `dotnet restore AgendaPro.sln`
  - `dotnet build AgendaPro.sln`
  - `dotnet test tests/AgendaPro.UnitTests/AgendaPro.UnitTests.csproj`
  - `dotnet test tests/AgendaPro.Tests/AgendaPro.Tests.csproj`
- Use `global.json` as the SDK baseline (`10.0.100` with `latestFeature` roll-forward) to keep local and CI builds consistent.
- Check target frameworks before editing project files. The solution is standardized on `net10.0`, including the test projects and `Directory.Build.props` defaults.

## Architecture

- Follow the existing layered flow: API -> Application -> Domain, with Infrastructure providing concrete implementations.
- Read [README.md](README.md) for the high-level structure and [src/AgendaPro.Domain/Shared/ResultPattern.md](src/AgendaPro.Domain/Shared/ResultPattern.md) for the result-handling pattern instead of restating them in code comments.
- Keep controllers thin. The current pattern is controller -> use case -> repository, with HTTP responses produced through `ResultExtensions.ToActionResult()` and `ApiResponse<T>`.
- New business flows should follow the existing feature slices:
  - Application: `DTOs` and `UseCase`/`UseCases`
  - Domain: `Models` and `Repositories`
  - Infrastructure: concrete repository implementations
  - API: controller endpoints
- Dependency injection is split across `src/AgendaPro.Infrastucture/InfrastructureExtensions.cs` and `src/AgendaPro.Api/Program.cs`. If you add a new feature, verify both registration points.

## Conventions That Matter

- Preserve the existing project and namespace spelling `AgendaPro.Infrastucture` unless the task is explicitly a repo-wide rename.
- Prefer `Result`/`Result<T>` plus `Error` for expected validation and business-rule failures. Do not introduce exceptions for normal invalid-input paths.
- Keep API responses consistent with the current wrapper pattern instead of returning raw entities or ad hoc anonymous objects.
- Stay localized when editing routes and naming. The current controllers mix explicit routes such as `api/tags` and `api/services` with `api/[controller]` for clients.
- Keep Portuguese domain wording and user-facing strings unless the task explicitly asks for translation or standardization.

## Validation

- Start with targeted validation for the project or file you touched before attempting a full-solution pass.
- Expect some code to be mid-refactor. Check existing errors in the touched area before assuming a new change introduced them.

## Useful Anchors

- `src/AgendaPro.Api/Program.cs`
- `src/AgendaPro.Api/Extensions/ResultExtensions.cs`
- `src/AgendaPro.Application/Tags/UseCase/TagUseCase.cs`
- `src/AgendaPro.Application/Services/UseCases/ServiceUseCase.cs`
- `src/AgendaPro.Application/Clients/UseCases/ClientUseCase.cs`
- `src/AgendaPro.Infrastucture/InfrastructureExtensions.cs`
