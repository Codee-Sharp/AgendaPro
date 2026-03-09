using AgendaPro.Application.Categories.DTOs;
using AgendaPro.Application.Tags.Dtos;
using AgendaPro.Domain.Services.Models;
using AgendaPro.Domain.Services.Repositories;
using AgendaPro.Domain.Shared;

namespace AgendaPro.Application.Categories.UseCases;

public class CategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;
    public CategoryUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<IEnumerable<CategoryDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);

        if(categories is null || !categories.Any())
            return Result<IEnumerable<CategoryDto>>.Failure(new Error("NOT_FOUND","No categories found"));
       
        var model = categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        });

        return Result<IEnumerable<CategoryDto>>.Success(model);
    }

    public async Task<Result<CategoryDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);

        if (category is null)
            return Result<CategoryDto>.Failure(new Error("NOT_FOUND", "Category not found"));

        var response = CategoryDto.FromModel(category);
        
        return Result<CategoryDto>.Success(response);
    }

    public async Task<Result<CategoryDto>> CreateAsync(CreateCategoryDto categoryDTO, CancellationToken cancellationToken)
    {

        var existingCategory = await _categoryRepository.GetByNameAsync(categoryDTO.Name, cancellationToken);

        if(existingCategory != null) 
            return Result<CategoryDto>.Failure(new Error("DUPLICATE_NAME", "A category with the same name already exists."));

        var userId = Guid.Empty; // enquanto nao implementa autenticacao

        var model = categoryDTO.ToModel(userId);
       
        _categoryRepository.Add(model);

        await _categoryRepository.SaveChangesAsync(cancellationToken);

        var response = CategoryDto.FromModel(model);

        return Result<CategoryDto>.Success(response);
    }
    
    public async Task<Result<CategoryDto>> UpdateAsync(Guid id, UpdateCategoryDto categoryDTO, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);

        if (category is null)
            return Result<CategoryDto>.Failure(new Error("NOT_FOUND", "Category not found"));

        category.Update(categoryDTO.Name, categoryDTO.Description);


        _categoryRepository.Update(category);

        await _categoryRepository.SaveChangesAsync(cancellationToken);

        var response = CategoryDto.FromModel(category);

        return Result<CategoryDto>.Success(response);
    }

    public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);

        if (category is null)
            return Result<bool>.Failure(new Error("NOT_FOUND", "Category not found"));

        _categoryRepository.Delete(category);

        await _categoryRepository.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
