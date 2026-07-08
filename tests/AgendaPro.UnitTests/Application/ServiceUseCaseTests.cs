using AgendaPro.Application.Services.DTOs;
using AgendaPro.Application.Services.UseCases;
using AgendaPro.Domain.Services.Models;
using AgendaPro.Domain.Services.Repositories;
using Moq;

namespace AgendaPro.UnitTests.Application;

public class ServiceUseCaseTests
{
    private readonly Mock<IServiceRepository> _repository = new();

    [Fact]
    public async Task CreateAsync_ShouldPersistAndReturnDto()
    {
        var sut = new ServiceUseCase(_repository.Object);
        var dto = CreateDto();

        var result = await sut.CreateAsync(dto);

        Assert.True(result.IsSuccess);
        Assert.Equal(dto.Nome, result.Value!.Nome);
        Assert.Equal(dto.Preco, result.Value.Preco);
        _repository.Verify(r => r.SaveAsync(It.Is<ServiceModel>(s => s.Nome == dto.Nome)), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailureWhenMissingAndSuccessWhenFound()
    {
        var sut = new ServiceUseCase(_repository.Object);
        var id = Guid.NewGuid();
        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ServiceModel?)null);

        var missing = await sut.GetByIdAsync(id);
        Assert.True(missing.IsFailure);

        var service = CreateModel();
        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(service);

        var result = await sut.GetByIdAsync(id);

        Assert.True(result.IsSuccess);
        Assert.Same(service, result.Value);
    }

    [Fact]
    public async Task GetAllAndFilters_ShouldReturnRepositoryItems()
    {
        var sut = new ServiceUseCase(_repository.Object);
        var service = CreateModel();
        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([service]);
        _repository.Setup(r => r.FilterByNameLike("Cor")).ReturnsAsync([service]);
        _repository.Setup(r => r.FilterByDescriptionLike("Premium")).ReturnsAsync([service]);

        var all = await sut.GetAllAsync();
        var byName = await sut.FilterByNameLike("Cor");
        var byDescription = await sut.FilterByDescriptionLike("Premium");

        Assert.Same(service, Assert.Single(all.Value!));
        Assert.Same(service, Assert.Single(byName.Value!));
        Assert.Same(service, Assert.Single(byDescription.Value!));
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFailureWhenMissingAndUpdateWhenFound()
    {
        var sut = new ServiceUseCase(_repository.Object);
        var id = Guid.NewGuid();
        var dto = CreateDto();
        dto.Nome = "Barba";
        dto.Preco = 80m;
        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ServiceModel?)null);

        var missing = await sut.UpdateAsync(id, dto);
        Assert.True(missing.IsFailure);

        var service = CreateModel();
        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(service);

        var result = await sut.UpdateAsync(id, dto);

        Assert.True(result.IsSuccess);
        Assert.Equal("Barba", service.Nome);
        Assert.Equal(80m, service.Preco);
        _repository.Verify(r => r.UpdateAsync(service), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFailureWhenMissingAndDeleteWhenFound()
    {
        var sut = new ServiceUseCase(_repository.Object);
        var id = Guid.NewGuid();
        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ServiceModel?)null);

        var missing = await sut.DeleteAsync(id);
        Assert.True(missing.IsFailure);

        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(CreateModel());

        var result = await sut.DeleteAsync(id);

        Assert.True(result.IsSuccess);
        Assert.True(result.Value);
        _repository.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    private static ServiceDto CreateDto() => new()
    {
        Nome = "Corte",
        DuracaoMin = 30,
        Preco = 50m,
        Descricao = "Premium",
        CategoriaId = 2,
        TempoIntervaloMin = 10
    };

    private static ServiceModel CreateModel() =>
        new("Corte", 30, 50m, "Premium", 2, 10);
}
