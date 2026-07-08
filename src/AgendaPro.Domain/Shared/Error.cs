using System;

namespace AgendaPro.Domain.Shared
{
    public sealed record Error(
        string Message,
        string? Field = null,
        IReadOnlyDictionary<string, string>? Metadata = null
    );
}
