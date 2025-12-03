using AgendaPro.Domain.Services.Models;
using System.ComponentModel.DataAnnotations;

namespace AgendaPro.Application.Categories.DTOs;

public sealed record CreateCategoryDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    [MinLength(3, ErrorMessage = "Name must have at least 3 characters")]
    public required string Name { get; init; }
    public string? Description { get; init; }

    public static CreateCategoryDto From(CategoryModel model)
    {
        return new CreateCategoryDto
        {
            Name = model.Name,
            Description = model.Description
        };
    }

    public CategoryModel ToModel(Guid createdBy)
    {
        return new CategoryModel(
            Name,
            Description,
            createdBy
        );
    }
}
