using AgendaPro.Domain.Shared;

namespace AgendaPro.UnitTests.Domain.Shared;

public class ErrorTests
{

    [Fact]
    public void Success_ShouldHaveNoErrors()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Failure_ShouldContainErrors()
    {
        var err = new Error("E1", "Erro");
        var result = Result.Failure(err);

        Assert.False(result.IsSuccess);
        Assert.Single(result.Errors);
        Assert.Equal("E1", result.Errors[0].Code);
    }

    [Fact]
    public void ResultT_Success_ShouldHaveValue()
    {
        var result = Result<string>.Success("Ruan");

        Assert.True(result.IsSuccess);
        Assert.Equal("Ruan", result.Value);
    }

    [Fact]
    public void ResultT_Failure_GetValueOrThrow_ShouldThrow()
    {
        var result = Result<string>.Failure(new Error("E1", "Erro"));

        Assert.False(result.IsSuccess);
        Assert.Throws<InvalidOperationException>(() => result.GetValueOrThrow());
    }

    [Fact]
    public void Combine_ShouldAggregateErrors()
    {
        var r1 = Result.Failure(new Error("E1", "Erro1"));
        var r2 = Result.Failure(new Error("E2", "Erro2"));
        var combined = Result.Combine(r1, r2);

        Assert.False(combined.IsSuccess);
        Assert.Equal(2, combined.Errors.Count);
    }
}
