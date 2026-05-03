---
description: "Use when working in AgendaPro.Api to keep controllers thin and preserve ResultExtensions and ApiResponse response conventions."
applyTo: "src/AgendaPro.Api/**/*.cs"
---

# AgendaPro API Conventions

- Keep controllers thin. Controllers should receive HTTP input, call a single application use case, and return the translated result.
- Prefer constructor injection or primary-constructor injection that matches the existing controller style in this project.
- Do not move business rules, persistence logic, or entity mutation into controllers. That belongs in Application use cases or deeper layers.
- For use case results, return `result.ToActionResult()` via `AgendaPro.Api.Extensions.ResultExtensions` instead of hand-building `Ok(...)`, `BadRequest(...)`, or ad hoc response objects.
- Keep HTTP responses in the `ApiResponse<T>` shape. Do not return raw domain entities, anonymous objects, or custom wrapper formats from controller actions.
- Treat `ApiResponseValidationFilter` as an enforcement point, not a fallback to rely on. New controller actions should already return the expected wrapper pattern without automatic correction.
- Keep route style consistent with the surrounding controller. This project currently mixes explicit routes like `api/tags` and `api/services` with `api/[controller]`.
- Use DTOs from the Application layer for request and response payloads. Do not expose Infrastructure types or persistence-specific types from API actions.
- Keep logging lightweight and request-focused in controllers. Avoid duplicating business validation messages or repository details at the API layer.
- If an endpoint needs a new feature flow, add or update the corresponding Application use case and only wire it into the controller once the use case contract is clear.

Useful anchors:

- [AGENTS.md](../../AGENTS.md)
- [src/AgendaPro.Api/Extensions/ResultExtensions.cs](../../src/AgendaPro.Api/Extensions/ResultExtensions.cs)
- [src/AgendaPro.Api/Wrappers/ApiResponse.cs](../../src/AgendaPro.Api/Wrappers/ApiResponse.cs)
- [src/AgendaPro.Api/Filters/ApiResponseValidationFilter.cs](../../src/AgendaPro.Api/Filters/ApiResponseValidationFilter.cs)
- [src/AgendaPro.Api/Controllers/TagController.cs](../../src/AgendaPro.Api/Controllers/TagController.cs)