---
description: "Use when working in AgendaPro.Application or AgendaPro.Domain to enforce Result/Result<T> for business and validation failures. Never throw exceptions for normal invalid-input paths."
applyTo: "src/AgendaPro.Application/**/*.cs|src/AgendaPro.Domain/**/*.cs"
---

# AgendaPro Result Pattern Instruction

- Always use `Result` or `Result<T>` (plus `Error`) to represent business-rule or validation failures in Application and Domain layers.
- Never throw exceptions for normal invalid-input or business-rule failures (e.g., not found, validation error, duplicate, etc.).
- Only throw exceptions for truly unexpected, unrecoverable errors (e.g., infrastructure failure, programming bug).
- This ensures all expected failures flow through the API as structured responses via `ResultExtensions.ToActionResult()`.
- See [src/AgendaPro.Domain/Shared/ResultPattern.md](../../src/AgendaPro.Domain/Shared/ResultPattern.md) for canonical usage and error shape.

## Example

```csharp
// Good
if (entity == null)
    return Result<T>.Failure(new Error("NotFound", "Entity not found"));

// Bad
if (entity == null)
    throw new KeyNotFoundException("Entity not found");
```

## Useful Anchors
- [src/AgendaPro.Domain/Shared/ResultPattern.md](../../src/AgendaPro.Domain/Shared/ResultPattern.md)
- [src/AgendaPro.Api/Extensions/ResultExtensions.cs](../../src/AgendaPro.Api/Extensions/ResultExtensions.cs)
- [AGENTS.md](../../AGENTS.md)
