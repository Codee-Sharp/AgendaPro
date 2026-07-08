using AgendaPro.Application.Clients.DTOs;
using AgendaPro.Application.Clients.UseCases;
using AgendaPro.Domain.Clients.Models;
using AgendaPro.Domain.Clients.Repositories;
using Moq;

namespace AgendaPro.UnitTests.Application;

public class ClientUseCaseTests
{
    private readonly Mock<IClientRepository> _repository = new();

    [Fact]
    public async Task CreateAsync_ShouldPersistAndReturnResponse()
    {
        var sut = new ClientUseCase(_repository.Object);
        var request = new CreateClientRequest
        {
            Name = "Ana",
            Email = "ana@test.com",
            Telephone = "11999999999",
            Observations = "Obs"
        };

        var result = await sut.CreateAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal(request.Name, result.Value!.Name);
        Assert.Equal(request.Email, result.Value.Email);
        _repository.Verify(r => r.SaveAsync(It.Is<ClientModel>(c => c.Name == request.Name)), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailureWhenMissingAndSuccessWhenFound()
    {
        var sut = new ClientUseCase(_repository.Object);
        var id = Guid.NewGuid();
        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ClientModel?)null);

        var missing = await sut.GetByIdAsync(id);
        Assert.True(missing.IsFailure);

        var client = CreateClient("Ana");
        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(client);

        var result = await sut.GetByIdAsync(id);

        Assert.True(result.IsSuccess);
        Assert.Equal(client.Id, result.Value!.Id);
    }

    [Fact]
    public async Task GetAllAndFilters_ShouldReturnRepositoryItems()
    {
        var sut = new ClientUseCase(_repository.Object);
        var client = CreateClient("Ana");
        _repository.Setup(r => r.GetAllAsync()).ReturnsAsync([client]);
        _repository.Setup(r => r.FilterByNameLike("An")).ReturnsAsync([client]);
        _repository.Setup(r => r.FilterByEmailLike("test")).ReturnsAsync([client]);

        var all = await sut.GetAllAsync();
        var byName = await sut.FilterByNameLike("An");
        var byEmail = await sut.FilterByEmailLike("test");

        Assert.Equal("Ana", Assert.Single(all.Value!).Name);
        Assert.Same(client, Assert.Single(byName.Value!));
        Assert.Same(client, Assert.Single(byEmail.Value!));
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFailureWhenMissingAndUpdateWhenFound()
    {
        var sut = new ClientUseCase(_repository.Object);
        var id = Guid.NewGuid();
        var request = new UpdateClientRequest
        {
            Name = "Maria",
            Email = "maria@test.com",
            Telephone = "11888888888",
            Observations = "Nova"
        };
        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ClientModel?)null);

        var missing = await sut.UpdateAsync(id, request);
        Assert.True(missing.IsFailure);

        var client = CreateClient("Ana");
        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(client);

        var result = await sut.UpdateAsync(id, request);

        Assert.True(result.IsSuccess);
        Assert.Equal("Maria", client.Name);
        Assert.Equal("maria@test.com", client.Email);
        _repository.Verify(r => r.UpdateAsync(client), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFailureWhenMissingAndDeleteWhenFound()
    {
        var sut = new ClientUseCase(_repository.Object);
        var id = Guid.NewGuid();
        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ClientModel?)null);

        var missing = await sut.DeleteAsync(id);
        Assert.True(missing.IsFailure);

        _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(CreateClient("Ana"));

        var result = await sut.DeleteAsync(id);

        Assert.True(result.IsSuccess);
        Assert.True(result.Value);
        _repository.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    private static ClientModel CreateClient(string name) =>
        new(name, $"{name.ToLowerInvariant()}@test.com", "11999999999", "Obs");
}
