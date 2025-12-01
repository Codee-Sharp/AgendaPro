using System.ComponentModel.DataAnnotations;

namespace AgendaPro.Application.Categories.DTOs;

public class CreateCategoryDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    [MinLength(3, ErrorMessage = "Name must have at least 2 characters")]
    public string Name { get; set; }
    public string? Description { get; set; }
}
