using System.ComponentModel.DataAnnotations;

namespace AgendaPro.Application.Categories.DTOs;

public sealed record class UpdateCategoryDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Name must have at least 3 characters")]
    public string? Name { get; init; }
    public string? Description { get; init; }
}
