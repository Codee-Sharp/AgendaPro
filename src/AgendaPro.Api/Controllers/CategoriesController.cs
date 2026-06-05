using AgendaPro.Api.Extensions;
using AgendaPro.Api.Wrappers;
using AgendaPro.Application.Categories.DTOs;
using AgendaPro.Application.Categories.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace AgendaPro.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryUseCase _categoryUseCase;
    public CategoriesController(CategoryUseCase categoryUseCase)
    {
        _categoryUseCase = categoryUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _categoryUseCase.GetAllAsync(cancellationToken);

        return result.ToActionResult();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _categoryUseCase.GetByIdAsync(id, cancellationToken);

        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> Create( CreateCategoryDto categoryDto, CancellationToken cancellationToken)
    {

        var result = await _categoryUseCase.CreateAsync(categoryDto, cancellationToken);

        return result.ToActionResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateCategoryDto categoryDto, CancellationToken cancellationToken)
    {
        var result = await _categoryUseCase.UpdateAsync(id, categoryDto, cancellationToken);

        return result.ToActionResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _categoryUseCase.DeleteAsync(id, cancellationToken);

        return result.ToActionResult();
    }
}
