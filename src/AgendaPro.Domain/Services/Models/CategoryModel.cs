using AgendaPro.Domain.Common;

namespace AgendaPro.Domain.Services.Models;

public class CategoryModel : BaseEntity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }

    public CategoryModel(string name, string? description)
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
