using System.ComponentModel.DataAnnotations;

namespace AgendaPro.Application.Categories.DTOs;

public class UpdateCategoryDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Name must have at least 2 characters")]
    public string? Name { get; set; }
    public string? Description { get; set; }
}
