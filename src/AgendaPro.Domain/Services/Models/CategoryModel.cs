using AgendaPro.Domain.Common;

namespace AgendaPro.Domain.Services.Models;

public class CategoryModel : AuditableEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }

    public CategoryModel(string name, string? description,  Guid createdBy) : base(createdBy)
    {
        Name = name;
        Description = description;
    }

    public void Update(string? name, string? description)
    {
            Name = name ?? Name;
            Description = description ?? Description;
    }
}
