using AgendaPro.Domain.Common;
using AgendaPro.Domain.Shared;

namespace AgendaPro.Domain.Professionals;

public class ProfessionalModel : AuditableEntity
{
    public string Name { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Specialty { get; private set; }
    public bool IsActive { get; private set; }

    private ProfessionalModel(Guid createdBy) : base(createdBy)
    {
    }

    public static Result<ProfessionalModel> Create(string name, string? email, string? phone, string? specialty, Guid createdBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<ProfessionalModel>.Failure(new Error("REQUIRED_FIELD", "Name is required."));

        var professional = new ProfessionalModel(createdBy)
        {
            Name = name,
            Email = email,
            Phone = phone,
            Specialty = specialty,
            IsActive = true
        };

        return Result<ProfessionalModel>.Success(professional);
    }

    public Result Update(string name, string? email, string? phone, string? specialty, Guid updatedBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(new Error("REQUIRED_FIELD", "Name is required."));

        Name = name;
        Email = email;
        Phone = phone;
        Specialty = specialty;

        MarkUpdated(DateTimeOffset.UtcNow, updatedBy);

        return Result.Success();
    }
    public void Deactivate(Guid deletedBy)
    {
        IsActive = false;
        SoftDelete(DateTimeOffset.UtcNow, deletedBy);
    }
    public void Reactivate(Guid reactivatedBy)
    {
        IsActive = true;
        Restore(DateTimeOffset.UtcNow, reactivatedBy);
    }
}
