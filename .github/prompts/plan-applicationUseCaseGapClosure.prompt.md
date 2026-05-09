## Plan: Application Use Case Gap Closure

Complete existing-slice parity first (Tags and Services), then treat roadmap slices (Appointment/Professional) as a second track so you get immediate API completeness without blocking on larger domain work.

**Steps**
1. Finalize target use-case matrix per slice using Clients as parity baseline.
2. Phase 1: Implement Tags CRUD use cases in Application and propagate required repository/API support.
3. Phase 1: Add Tags query use cases (GetAll, GetById, optional FilterByName) and wire through stack.
4. Phase 1: Add Service filters (name/description) and wire through stack.
5. Phase 1: Normalize Services not-found handling to Result failures (remove exception flow).
6. Phase 2: Scaffold roadmap-level slices (Appointment, Professional) with minimal use-case contracts.
7. Validate with restore/build/tests and manual endpoint checks.

**Relevant files**
- src/AgendaPro.Application/Tags/UseCase/TagUseCase.cs
- src/AgendaPro.Application/Services/UseCases/ServiceUseCase.cs
- src/AgendaPro.Application/Clients/UseCases/ClientUseCase.cs
- src/AgendaPro.Domain/Tags/Repositories/ITagRepository.cs
- src/AgendaPro.Domain/Services/Repositories/IServiceRepository.cs
- src/AgendaPro.Infrastucture/Tags/TagRepository.cs
- src/AgendaPro.Infrastucture/Services/ServiceRepository.cs
- src/AgendaPro.Api/Controllers/TagController.cs
- src/AgendaPro.Api/Controllers/ServicesController.cs
- src/AgendaPro.Api/Program.cs
- src/AgendaPro.Infrastucture/InfrastructureExtensions.cs

**Verification**
1. dotnet restore AgendaPro.sln
2. dotnet build AgendaPro.sln
3. dotnet test tests/AgendaPro.UnitTests/AgendaPro.UnitTests.csproj
4. dotnet test tests/AgendaPro.Tests/AgendaPro.Tests.csproj
5. Manual checks in AgendaPro.Api.http for new Tags and Services endpoints