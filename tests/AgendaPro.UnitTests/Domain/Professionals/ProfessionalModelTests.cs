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

        // Act
        var result = ProfessionalModel.Create(name, email, phone, specialty);

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

        // Act
        var result = ProfessionalModel.Create(name, email, phone, specialty);

        // Assert
        Assert.True(result.IsFailure);
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
        var createResult = ProfessionalModel.Create(name, email, phone, specialty);

        var professional = createResult.Value;

        var updatedName = "Profissional Atualizado";
        // Act
        var updateResult = professional.Update(updatedName, email, phone, specialty);

        // Assert
        Assert.True(updateResult.IsSuccess);
        Assert.Equal(updatedName, professional.Name);
    }

    [Fact]
    public void Update_ShouldReturnFailure_WhenNameIsEmpty()
    {
        // Arrange
        var professional = ProfessionalModel.Create("Profissional", null, null, null).Value;

        // Act
        var result = professional.Update("", null, null, null);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Name is required.", result.Errors.FirstOrDefault().Message);
    }

    [Fact]
    public void Deactivate_ShouldDeactivateProfessional_WhenProfessionalIsActive()
    {
        // Arrange
        var professional = ProfessionalModel.Create("Profissional", "profissional@email.com", "81999999999", "especialidade").Value;

        // Act
        professional.Deactivate();

        // Assert
        Assert.False(professional.IsActive);
    }

    [Fact]
    public void Reactivate_ShouldActivateProfessional_WhenProfessionalIsInactive()
    {
        // Arrange
        var professional = ProfessionalModel.Create("Profissional", "profissional@email.com", "81999999999", "especialidade").Value;

        professional.Deactivate();

        // Act
         professional.Reactivate();

        // Assert
        Assert.True(professional.IsActive);
    }
}
