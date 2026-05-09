---
name: add-feature-slice
description: 'Scaffold a new AgendaPro feature slice across Api, Application, Domain, and Infrastucture. Use when adding a feature like Tags, Services, or Clients and you need DTOs, use cases, repositories, controllers, and DI registrations without missing the split registration points.'
argument-hint: 'Feature name, singular/plural naming, and whether the slice is create-only or full CRUD'
user-invocable: true
---

# Add Feature Slice

Use this skill when you need to add a new backend feature that follows the existing AgendaPro slice pattern across:

- `src/AgendaPro.Api`
- `src/AgendaPro.Application`
- `src/AgendaPro.Domain`
- `src/AgendaPro.Infrastucture`

The goal is to scaffold the feature consistently and avoid a common failure mode in this repo: adding the slice files but forgetting one of the DI registration points.

## When To Use

- Add a new feature similar to Tags, Services, or Clients
- Expand a minimal slice into a fuller CRUD slice
- Add a repository and use case pair that must be reachable from the API layer
- Add a controller that must preserve `ResultExtensions.ToActionResult()` and `ApiResponse<T>` behavior

## Inputs To Confirm

Before editing, identify:

1. Feature name in singular and plural form
2. Folder and namespace shape to match existing slices
3. Endpoint scope:
   - minimal endpoint set (for constrained MVP behavior)
   - CRUD, like Services or Clients
   - CRUD plus filters or custom queries, like Clients and Tags
4. DTO strategy:
   - one DTO reused for input and output
   - separate request and response DTOs
5. Repository registration needs in both `InfrastructureExtensions` and `Program.cs`

If the user does not specify CRUD depth, default to the smallest slice that matches the requested behavior.

## Procedure

1. Find the closest existing slice.
   - Use Tags for CRUD plus filter endpoint pattern.
   - Use Services or Clients for fuller CRUD controllers and use cases.
   - Preserve the repo's existing spelling `AgendaPro.Infrastucture`.

2. Create or extend the Domain layer first.
   - Add the feature folder under `src/AgendaPro.Domain/<Feature>/`.
   - Add `Models` and `Repositories` as needed.
   - Define the repository interface in Domain.
   - Keep domain models free of API and infrastructure concerns.

3. Add the Application slice.
   - Add DTOs under `src/AgendaPro.Application/<Feature>/DTOs` or `Dtos`, matching the local slice convention.
   - Add a `UseCase` or `UseCases` folder that matches the neighboring slice.
   - Return `Result` or `Result<T>` from use case methods.
   - Put orchestration and validation here, not in controllers.

4. Add the Infrastructure implementation.
   - Add the concrete repository under `src/AgendaPro.Infrastucture/<Feature>/`.
   - Implement the Domain repository contract.
   - If persistence support is needed, wire it to the existing data context pattern.

5. Add or update the API controller.
   - Place the controller under `src/AgendaPro.Api/Controllers/`.
   - Keep the controller thin: map HTTP input, call one use case, return the result.
   - Use `result.ToActionResult()` from `AgendaPro.Api.Extensions.ResultExtensions`.
   - Do not return raw entities or custom wrappers.
   - Follow the surrounding route style instead of forcing a new global route convention.

6. Register dependencies in both places.
   - Update `src/AgendaPro.Infrastucture/InfrastructureExtensions.cs` when repository registration belongs with infrastructure setup.
   - Update `src/AgendaPro.Api/Program.cs` for explicit use case and repository registrations already wired there.
   - Verify that the new feature can be resolved end-to-end from controller to repository.

7. Validate narrowly.
   - Build the touched project first.
   - If possible, run the most relevant test project for the touched slice.
   - Check for missing namespaces, folder-name mismatches, and DI omissions before widening scope.

## Decision Points

### Choose the slice depth

- If the feature only needs a minimal subset, implement only the required operations and keep DI wired in both registration points.
- If it needs standard CRUD, mirror Services.
- If it needs CRUD plus filter endpoints, mirror Clients or Tags.

### Choose DTO placement and naming

- Match the nearest existing slice even if naming is inconsistent across the repo.
- Do not normalize `DTOs` vs `Dtos` or `UseCase` vs `UseCases` unless the task is explicitly a cleanup.

### Choose error handling

- Use `Result.Failure(...)` or `Result<T>.Failure(...)` for expected business or validation failures.
- Avoid introducing exceptions for normal invalid-input paths.
- Preserve Portuguese user-facing text unless the task explicitly asks for translation.

## Completion Checks

The skill is complete when all of the following are true:

- Domain model and repository contract exist where the slice expects them
- Application DTOs and use case methods compile against the Domain contract
- Infrastructure repository implements the Domain interface
- API controller delegates to the use case and returns `result.ToActionResult()`
- DI is updated in every required registration point
- The touched project or slice passes a targeted build or test check

## Common Failure Modes

- Creating the repository class but forgetting the interface in Domain
- Registering the repository in `InfrastructureExtensions` but not wiring the use case in `Program.cs`
- Returning `Ok(...)` or `BadRequest(...)` directly instead of the existing `ApiResponse<T>` flow
- Mixing new naming conventions into a slice that already has an established local pattern
- Accidentally “fixing” `AgendaPro.Infrastucture` spelling during unrelated feature work

## Useful Anchors

- `src/AgendaPro.Api/Program.cs`
- `src/AgendaPro.Api/Extensions/ResultExtensions.cs`
- `src/AgendaPro.Api/Controllers/TagController.cs`
- `src/AgendaPro.Api/Controllers/ServicesController.cs`
- `src/AgendaPro.Api/Controllers/ClientsController.cs`
- `src/AgendaPro.Application/Tags/UseCase/TagUseCase.cs`
- `src/AgendaPro.Application/Services/UseCases/ServiceUseCase.cs`
- `src/AgendaPro.Application/Clients/UseCases/ClientUseCase.cs`
- `src/AgendaPro.Infrastucture/InfrastructureExtensions.cs`

## Example Prompts

- `/add-feature-slice appointments full CRUD with filters by date and professional`
- `/add-feature-slice categories create-only slice matching Tags`
- `/add-feature-slice professionals CRUD slice with DTOs and DI wiring`