using AgendaPro.Application.Tags.Dtos;
using AgendaPro.Application.Tags.UseCase;
using AgendaPro.Domain.Tags.Repositories;
using Moq;

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
        Assert.Equal("Nova Tag", result.Value.Name);
    }
}
