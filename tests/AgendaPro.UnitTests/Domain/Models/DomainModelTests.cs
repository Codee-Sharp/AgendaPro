using AgendaPro.Domain.Clients.Models;
using AgendaPro.Domain.Common;
using AgendaPro.Domain.Services.Models;
using AgendaPro.Domain.Tags.Models;

namespace AgendaPro.UnitTests.Domain.Models;

public class DomainModelTests
{
    [Fact]
    public void ClientModel_ShouldCreateAndUpdate()
    {
        var createdBy = Guid.NewGuid();
        var client = new ClientModel("Ana", "ana@test.com", "11999999999", "Inicial", createdBy);

        Assert.Equal("Ana", client.Name);
        Assert.Equal("ana@test.com", client.Email);
        Assert.Equal("11999999999", client.Telephone);
        Assert.Equal("Inicial", client.Observations);
        Assert.Equal(createdBy, client.CreatedBy);

        client.Update("Maria", "maria@test.com", "11888888888", "Atualizado");

        Assert.Equal("Maria", client.Name);
        Assert.Equal("maria@test.com", client.Email);
        Assert.Equal("11888888888", client.Telephone);
        Assert.Equal("Atualizado", client.Observations);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void ClientModel_ShouldRejectEmptyName(string? name)
    {
        Assert.Throws<ArgumentException>(
            () => new ClientModel(name!, null, null, null, Guid.NewGuid()));
    }

    [Fact]
    public void CategoryModel_ShouldCreateAndUpdateOnlyProvidedValues()
    {
        var category = new CategoryModel("Cabelo", "Descrição", Guid.NewGuid());

        Assert.Equal("Cabelo", category.Name);
        Assert.Equal("Descrição", category.Description);

        category.Update("Barba", null);
        Assert.Equal("Barba", category.Name);
        Assert.Equal("Descrição", category.Description);

        category.Update(null, "Nova descrição");
        Assert.Equal("Barba", category.Name);
        Assert.Equal("Nova descrição", category.Description);
    }

    [Fact]
    public void ServiceModel_ShouldCreateUsingBothConstructorsAndUpdate()
    {
        var createdBy = Guid.NewGuid();
        var simple = new ServiceModel("Corte", createdBy);

        Assert.Equal("Corte", simple.Nome);
        Assert.Equal(createdBy, simple.CreatedBy);

        var service = new ServiceModel("Barba", 30, 50m, "Completa", 2, 10, createdBy);

        Assert.Equal("Barba", service.Nome);
        Assert.Equal(30, service.DuracaoMin);
        Assert.Equal(50m, service.Preco);
        Assert.Equal("Completa", service.Descricao);
        Assert.Equal(2, service.CategoriaId);
        Assert.Equal(10, service.TempoIntervaloMin);

        service.UpdateService("Cabelo", 45, 75m, "Premium", 3, 15);
        var category = new CategoryModel("Categoria", null, createdBy);
        service.Category = category;

        Assert.Equal("Cabelo", service.Nome);
        Assert.Equal(45, service.DuracaoMin);
        Assert.Equal(75m, service.Preco);
        Assert.Equal("Premium", service.Descricao);
        Assert.Equal(3, service.CategoriaId);
        Assert.Equal(15, service.TempoIntervaloMin);
        Assert.Same(category, service.Category);
    }

    [Fact]
    public void TagModel_ShouldUpdateDisableAndEnable()
    {
        var tag = new TagModel("Inicial", Guid.NewGuid());
        var disabledBy = Guid.NewGuid();

        tag.UpdateName("Atualizada");
        tag.Disable(disabledBy);

        Assert.Equal("Atualizada", tag.Name);
        Assert.Equal(disabledBy, tag.DisabedBy);
        Assert.NotNull(tag.DisabedAt);

        tag.Enable();

        Assert.Null(tag.DisabedBy);
        Assert.Null(tag.DisabedAt);
    }

    [Fact]
    public void AuditableEntity_ShouldTrackCreationUpdateAndDeletion()
    {
        var entity = new TestAuditableEntity(Guid.NewGuid(), Guid.NewGuid());
        var createdAt = DateTimeOffset.UtcNow.AddDays(-2);
        var updatedAt = DateTimeOffset.UtcNow.AddDays(-1);
        var deletedAt = DateTimeOffset.UtcNow;
        var createdBy = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var deletedBy = Guid.NewGuid();

        entity.MarkCreated(createdAt, createdBy);
        Assert.Equal(createdAt, entity.CreatedAt);
        Assert.Equal(createdBy, entity.UpdatedBy);
        Assert.False(entity.IsDeleted);

        entity.MarkUpdated(updatedAt, updatedBy);
        Assert.Equal(updatedAt, entity.UpdetadAt);
        Assert.Equal(updatedBy, entity.UpdatedBy);

        entity.SoftDelete(deletedAt, deletedBy);
        Assert.True(entity.IsDeleted);
        Assert.Equal(deletedAt, entity.DeletedAt);
        Assert.Equal(deletedBy, entity.DeletedBy);
    }

    private sealed class TestAuditableEntity(Guid id, Guid createdBy)
        : AuditableEntity<Guid>(id, createdBy);
}
