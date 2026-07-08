using AgendaPro.Application.Categories.DTOs;
using AgendaPro.Application.Categories.UseCases;
using AgendaPro.Domain.Services.Models;
using AgendaPro.Domain.Services.Repositories;
using Moq;

namespace AgendaPro.UnitTests.Application;

public class CategoryUseCaseTests
{
    private readonly Mock<ICategoryRepository> _repository = new();

    [Fact]
    public async Task GetAllAsync_ShouldReturnFailureForEmptyListAndSuccessForItems()
    {
        var sut = new CategoryUseCase(_repository.Object);
        _repository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var emptyResult = await sut.GetAllAsync(CancellationToken.None);

        Assert.True(emptyResult.IsFailure);
        Assert.Equal("No categories found", emptyResult.Errors[0].Message);

        var category = new CategoryModel("Cabelo", "Descrição");
        _repository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([category]);

        var result = await sut.GetAllAsync(CancellationToken.None);

        Assert.True(result.IsSuccess);
        var dto = Assert.Single(result.Value!);
        Assert.Equal(category.Id, dto.Id);
        Assert.Equal("Cabelo", dto.Name);
        Assert.Equal("Descrição", dto.Description);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailureWhenMissingAndSuccessWhenFound()
    {
        var sut = new CategoryUseCase(_repository.Object);
        var id = Guid.NewGuid();

        _repository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CategoryModel?)null);

        var missing = await sut.GetByIdAsync(id, CancellationToken.None);
        Assert.True(missing.IsFailure);

        var category = new CategoryModel("Cabelo", null);
        _repository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var result = await sut.GetByIdAsync(id, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(category.Id, result.Value!.Id);
    }

    [Fact]
    public async Task CreateAsync_ShouldRejectDuplicateAndPersistNewCategory()
    {
        var sut = new CategoryUseCase(_repository.Object);
        var dto = new CreateCategoryDto { Name = "Cabelo", Description = "Descrição" };
        _repository.Setup(r => r.GetByNameAsync(dto.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategoryModel(dto.Name, null));

        var duplicate = await sut.CreateAsync(dto, CancellationToken.None);

        Assert.True(duplicate.IsFailure);
        Assert.Equal("A category with the same name already exists.", duplicate.Errors[0].Message);

        _repository.Setup(r => r.GetByNameAsync(dto.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CategoryModel?)null);

        var result = await sut.CreateAsync(dto, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(dto.Name, result.Value!.Name);
        _repository.Verify(r => r.Add(It.Is<CategoryModel>(c => c.Name == dto.Name)), Times.Once);
        _repository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFailureWhenMissingAndUpdateWhenFound()
    {
        var sut = new CategoryUseCase(_repository.Object);
        var id = Guid.NewGuid();
        var dto = new UpdateCategoryDto { Name = "Barba", Description = "Nova" };
        _repository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CategoryModel?)null);

        var missing = await sut.UpdateAsync(id, dto, CancellationToken.None);
        Assert.True(missing.IsFailure);

        var category = new CategoryModel("Cabelo", "Antiga");
        _repository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var result = await sut.UpdateAsync(id, dto, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("Barba", result.Value!.Name);
        Assert.Equal("Nova", result.Value.Description);
        _repository.Verify(r => r.Update(category), Times.Once);
        _repository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFailureWhenMissingAndDeleteWhenFound()
    {
        var sut = new CategoryUseCase(_repository.Object);
        var id = Guid.NewGuid();
        _repository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CategoryModel?)null);

        var missing = await sut.DeleteAsync(id, CancellationToken.None);
        Assert.True(missing.IsFailure);

        var category = new CategoryModel("Cabelo", null);
        _repository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var result = await sut.DeleteAsync(id, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.True(result.Value);
        _repository.Verify(r => r.Delete(category), Times.Once);
        _repository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
