using AgendaPro.Domain.Services.Models;

namespace AgendaPro.Application.Categories.DTOs;

public sealed record class CategoryDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }

    public static CategoryDto FromModel(CategoryModel category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }
}