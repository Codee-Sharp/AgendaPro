using AgendaPro.Application.Categories.DTOs;
using AgendaPro.Application.Clients.DTOs;
using AgendaPro.Application.Services.DTOs;
using AgendaPro.Application.Tags.Dtos;
using AgendaPro.Domain.Clients.Models;
using AgendaPro.Domain.Services.Models;
using AgendaPro.Domain.Tags.Models;

namespace AgendaPro.UnitTests.Application;

public class DtoTests
{
    [Fact]
    public void CategoryDtos_ShouldMapInBothDirections()
    {
        var model = new CategoryModel("Cabelo", "Descrição");

        var dto = CategoryDto.FromModel(model);
        var createDto = CreateCategoryDto.From(model);
        var recreated = createDto.ToModel();
        var updateDto = new UpdateCategoryDto { Name = "Barba", Description = "Nova" };

        Assert.Equal(model.Id, dto.Id);
        Assert.Equal("Cabelo", dto.Name);
        Assert.Equal("Descrição", dto.Description);
        Assert.Equal("Cabelo", createDto.Name);
        Assert.Equal("Descrição", createDto.Description);
        Assert.Equal("Cabelo", recreated.Name);
        Assert.Equal("Descrição", recreated.Description);
        Assert.Equal("Barba", updateDto.Name);
        Assert.Equal("Nova", updateDto.Description);
    }

    [Fact]
    public void ClientDtos_ShouldExposeAndMapAllProperties()
    {
        var model = new ClientModel("Ana", "ana@test.com", "11999999999", "Obs");
        var dto = new ClientDto(model);
        var emptyDto = new ClientDto
        {
            Id = model.Id,
            Name = "Maria",
            Email = "maria@test.com",
            Telephone = "11888888888",
            Observations = "Outra"
        };
        var response = new ClientResponse(model);
        var create = new CreateClientRequest
        {
            Name = "João",
            Email = "joao@test.com",
            Telephone = "11777777777",
            Observations = "Criar"
        };
        var update = new UpdateClientRequest
        {
            Name = "José",
            Email = "jose@test.com",
            Telephone = "11666666666",
            Observations = "Atualizar"
        };

        Assert.Equal(model.Id, dto.Id);
        Assert.Equal(model.Name, dto.Name);
        Assert.Equal(model.Email, dto.Email);
        Assert.Equal(model.Telephone, dto.Telephone);
        Assert.Equal(model.Observations, dto.Observations);
        Assert.Equal("Maria", emptyDto.Name);
        Assert.Equal("maria@test.com", emptyDto.Email);
        Assert.Equal("11888888888", emptyDto.Telephone);
        Assert.Equal("Outra", emptyDto.Observations);
        Assert.Equal(model.Id, response.Id);
        Assert.Equal(model.Name, response.Name);
        Assert.Equal(model.Email, response.Email);
        Assert.Equal(model.Telephone, response.Telephone);
        Assert.Equal(model.Observations, response.Observations);
        Assert.Equal("João", create.Name);
        Assert.Equal("joao@test.com", create.Email);
        Assert.Equal("11777777777", create.Telephone);
        Assert.Equal("Criar", create.Observations);
        Assert.Equal("José", update.Name);
        Assert.Equal("jose@test.com", update.Email);
        Assert.Equal("11666666666", update.Telephone);
        Assert.Equal("Atualizar", update.Observations);
    }

    [Fact]
    public void ServiceAndTagDtos_ShouldExposeAndMapAllProperties()
    {
        var serviceModel = new ServiceModel("Corte", 30, 50m, "Descrição", 2, 10);
        var serviceDto = new ServiceDto(serviceModel);
        var emptyServiceDto = new ServiceDto
        {
            Id = serviceModel.Id,
            Nome = "Barba",
            DuracaoMin = 20,
            Preco = 35m,
            Descricao = "Nova",
            CategoriaId = 3,
            TempoIntervaloMin = 5
        };
        var tagModel = new TagModel("VIP");
        var tagDto = new TagDto(tagModel);
        var emptyTagDto = new TagDto { Id = tagModel.Id, Name = "Premium" };

        Assert.Equal(serviceModel.Id, serviceDto.Id);
        Assert.Equal(serviceModel.Nome, serviceDto.Nome);
        Assert.Equal(serviceModel.DuracaoMin, serviceDto.DuracaoMin);
        Assert.Equal(serviceModel.Preco, serviceDto.Preco);
        Assert.Equal(serviceModel.Descricao, serviceDto.Descricao);
        Assert.Equal(serviceModel.CategoriaId, serviceDto.CategoriaId);
        Assert.Equal(serviceModel.TempoIntervaloMin, serviceDto.TempoIntervaloMin);
        Assert.Equal("Barba", emptyServiceDto.Nome);
        Assert.Equal(20, emptyServiceDto.DuracaoMin);
        Assert.Equal(35m, emptyServiceDto.Preco);
        Assert.Equal("Nova", emptyServiceDto.Descricao);
        Assert.Equal(3, emptyServiceDto.CategoriaId);
        Assert.Equal(5, emptyServiceDto.TempoIntervaloMin);
        Assert.Equal(tagModel.Id, tagDto.Id);
        Assert.Equal("VIP", tagDto.Name);
        Assert.Equal(tagModel.Id, emptyTagDto.Id);
        Assert.Equal("Premium", emptyTagDto.Name);
    }
}
