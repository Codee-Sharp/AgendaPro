using AgendaPro.Domain.Professionals;

namespace AgendaPro.UnitTests.Domain.Professionals;

public class ProfessionalModelTests
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var name = "profissional";
        var email = "profissional@email.com";
        var phone = "81999999999";
        var specialty = "especialidade";
        var createdBy = Guid.NewGuid();

        // Act
        var result = ProfessionalModel.Create(name, email, phone, specialty, createdBy);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.IsActive);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenNameIsEmpty()
    {
        // Arrange
        var name = "";
        var email = "profissional@email.com";
        var phone = "81999999999";
        var specialty = "especialidade";
        var createdBy = Guid.NewGuid();

        // Act
        var result = ProfessionalModel.Create(name, email, phone, specialty, createdBy);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("REQUIRED_FIELD", result.Errors.FirstOrDefault().Code);
        Assert.Equal("Name is required.", result.Errors.FirstOrDefault().Message);
    }

    [Fact]
    public void Update_ShouldReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var name = "Profissional";
        var email = "profissional@email.com";
        var phone = "81999999999";
        var specialty = "especialidade";
        var createdBy = Guid.NewGuid();

        var createResult = ProfessionalModel.Create(name, email, phone, specialty, createdBy);

        var professional = createResult.Value;

        var updatedName = "Profissional Atualizado";
        var updatedBy = Guid.NewGuid();

        // Act
        var updateResult = professional.Update(updatedName, email, phone, specialty, updatedBy);

        // Assert
        Assert.True(updateResult.IsSuccess);
        Assert.Equal(updatedName, professional.Name);
        Assert.Equal(updatedBy, professional.UpdatedBy);
        Assert.NotNull(professional.UpdatedAt);
    }

    [Fact]
    public void Update_ShouldReturnFailure_WhenNameIsEmpty()
    {
        // Arrange
        var professional = ProfessionalModel.Create("Profissional", null, null, null, Guid.NewGuid()).Value;

        // Act
        var result = professional.Update("", null, null, null, Guid.NewGuid());

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("REQUIRED_FIELD", result.Errors.First().Code);
        Assert.Equal("Name is required.", result.Errors.FirstOrDefault().Message);
    }

    [Fact]
    public void Deactivate_ShouldDeactivateProfessional_WhenProfessionalIsActive()
    {
        // Arrange
        var professional = ProfessionalModel.Create("Profissional", "profissional@email.com", "81999999999", "especialidade", Guid.NewGuid()).Value;

        var DeletedBy = Guid.NewGuid();

        // Act
        professional.Deactivate(DeletedBy);

        // Assert
        Assert.False(professional.IsActive);
        Assert.True(professional.IsDeleted);
        Assert.Equal(DeletedBy, professional.DeletedBy);
        Assert.NotNull(professional.DeletedAt);
        Assert.NotNull(professional.DeletedAt);
        Assert.Equal(DeletedBy, professional.UpdatedBy);
        Assert.NotNull(professional.UpdatedAt);
    }

    [Fact]
    public void Reactivate_ShouldActivateProfessional_WhenProfessionalIsInactive()
    {
        // Arrange
        var professional = ProfessionalModel.Create("Profissional", "profissional@email.com", "81999999999", "especialidade", Guid.NewGuid()).Value;

        professional.Deactivate(Guid.NewGuid());

        var reactivatedBy = Guid.NewGuid();

        // Act
         professional.Reactivate(reactivatedBy);

        // Assert
        Assert.True(professional.IsActive);
        Assert.False(professional.IsDeleted);
        Assert.Equal(reactivatedBy, professional.UpdatedBy);
        Assert.NotNull(professional.UpdatedAt);
        Assert.Null(professional.DeletedAt);
        Assert.Null(professional.DeletedBy);
    }
}
