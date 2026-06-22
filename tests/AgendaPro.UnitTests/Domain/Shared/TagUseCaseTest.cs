using AgendaPro.Application.Tags.Dtos;
using AgendaPro.Application.Tags.UseCase;
using AgendaPro.Domain.Tags.Models;
using AgendaPro.Domain.Tags.Repositories;
using Moq;

namespace AgendaPro.UnitTests.Tags;

public class TagUseCaseTest
{
    [Fact]
    public async Task CreateAsync_ShouldReturnFailure_WhenNameIsEmpty()
    {
        // Arrange
        var repoMock = new Mock<ITagRepository>();
        var useCase = new TagUseCase(repoMock.Object);

        var dto = new TagDto { Name = "" };

        // Act
        var result = await useCase.CreateAsync(dto);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("O nome da tag é obrigatório", result.Errors.Select(e => e.Message));
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccess_WhenNameIsValid()
    {
        // Arrange
        var repoMock = new Mock<ITagRepository>();
        var useCase = new TagUseCase(repoMock.Object);

        var dto = new TagDto { Name = "Nova Tag" };

        // Act
        var result = await useCase.CreateAsync(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Nova Tag", result.Value!.Name);
        repoMock.Verify(r => r.SaveAsync(It.Is<TagModel>(t => t.Name == "Nova Tag")), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailureWhenMissingAndSuccessWhenFound()
    {
        var repoMock = new Mock<ITagRepository>();
        var useCase = new TagUseCase(repoMock.Object);
        var id = Guid.NewGuid();
        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TagModel?)null);

        var missing = await useCase.GetByIdAsync(id);
        Assert.True(missing.IsFailure);

        var tag = new TagModel("VIP", Guid.NewGuid());
        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(tag);

        var result = await useCase.GetByIdAsync(id);

        Assert.True(result.IsSuccess);
        Assert.Equal(tag.Id, result.Value!.Id);
    }

    [Fact]
    public async Task GetAllAndFilter_ShouldMapRepositoryItems()
    {
        var repoMock = new Mock<ITagRepository>();
        var useCase = new TagUseCase(repoMock.Object);
        var tag = new TagModel("VIP", Guid.NewGuid());
        repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync([tag]);
        repoMock.Setup(r => r.FilterByNameLike("VI")).ReturnsAsync([tag]);

        var all = await useCase.GetAllAsync();
        var filtered = await useCase.FilterByNameLike("VI");

        Assert.Equal("VIP", Assert.Single(all.Value!).Name);
        Assert.Equal("VIP", Assert.Single(filtered.Value!).Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldValidateNameHandleMissingAndUpdate()
    {
        var repoMock = new Mock<ITagRepository>();
        var useCase = new TagUseCase(repoMock.Object);
        var id = Guid.NewGuid();

        var invalid = await useCase.UpdateAsync(id, new TagDto { Name = " " });
        Assert.True(invalid.IsFailure);

        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TagModel?)null);
        var missing = await useCase.UpdateAsync(id, new TagDto { Name = "Premium" });
        Assert.True(missing.IsFailure);

        var tag = new TagModel("VIP", Guid.NewGuid());
        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(tag);

        var result = await useCase.UpdateAsync(id, new TagDto { Name = "Premium" });

        Assert.True(result.IsSuccess);
        Assert.Equal("Premium", tag.Name);
        repoMock.Verify(r => r.UpdateAsync(tag), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFailureWhenMissingAndDeleteWhenFound()
    {
        var repoMock = new Mock<ITagRepository>();
        var useCase = new TagUseCase(repoMock.Object);
        var id = Guid.NewGuid();
        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TagModel?)null);

        var missing = await useCase.DeleteAsync(id);
        Assert.True(missing.IsFailure);

        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new TagModel("VIP", Guid.NewGuid()));

        var result = await useCase.DeleteAsync(id);

        Assert.True(result.IsSuccess);
        repoMock.Verify(r => r.DeleteAsync(id), Times.Once);
    }
}
