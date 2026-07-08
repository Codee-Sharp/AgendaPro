using AgendaPro.Domain.Common;
using AgendaPro.Domain.Shared;

namespace AgendaPro.Domain.Professionals;

public class ProfessionalModel : BaseEntity
{
    public string Name { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Specialty { get; private set; }
    public bool IsActive { get; private set; }

    private ProfessionalModel()
    {
    }

    public static Result<ProfessionalModel> Create(string name, string? email, string? phone, string? specialty)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<ProfessionalModel>.Failure(new Error("Name is required."));

        var professional = new ProfessionalModel()
        {
            Name = name,
            Email = email,
            Phone = phone,
            Specialty = specialty,
            IsActive = true
        };

        return Result<ProfessionalModel>.Success(professional);
    }

    public Result Update(string name, string? email, string? phone, string? specialty)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(new Error("Name is required."));

        Name = name;
        Email = email;
        Phone = phone;
        Specialty = specialty;

        return Result.Success();
    }
    public void Deactivate()
    {
        IsActive = false;
    }
    public void Reactivate()
    {
        IsActive = true;
    }
}
