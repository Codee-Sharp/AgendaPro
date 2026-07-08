using AgendaPro.Domain.Shared;

namespace AgendaPro.UnitTests.Domain.Shared;

public class ErrorTests
{
    [Fact]
    public void Error_ShouldExposeOptionalDetails()
    {
        var metadata = new Dictionary<string, string> { ["value"] = "invalid" };
        var error = new Error("Erro", "Name", metadata);

        Assert.Equal("Erro", error.Message);
        Assert.Equal("Name", error.Field);
        Assert.Same(metadata, error.Metadata);
    }

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
        var err = new Error("Erro");
        var result = Result.Failure(err);

        Assert.False(result.IsSuccess);
        Assert.Single(result.Errors);
        Assert.Equal("Erro", result.Errors[0].Message);
    }

    [Fact]
    public void ResultT_Success_ShouldHaveValue()
    {
        var result = Result<string>.Success("Ruan");

        Assert.True(result.IsSuccess);
        Assert.Equal("Ruan", result.Value);
        Assert.Equal("Ruan", result.GetValueOrThrow());
    }

    [Fact]
    public void ResultT_Failure_GetValueOrThrow_ShouldThrow()
    {
        var result = Result<string>.Failure(new Error("Erro"));

        Assert.False(result.IsSuccess);
        Assert.Throws<InvalidOperationException>(() => result.GetValueOrThrow());
    }

    [Fact]
    public void Combine_ShouldAggregateErrors()
    {
        var r1 = Result.Failure(new Error("Erro1"));
        var r2 = Result.Failure(new Error("Erro2"));
        var combined = Result.Combine(r1, r2);

        Assert.False(combined.IsSuccess);
        Assert.Equal(2, combined.Errors.Count);
    }

    [Fact]
    public void Combine_ShouldReturnSuccessWhenAllResultsSucceed()
    {
        var combined = Result.Combine(Result.Success(), Result.Success());

        Assert.True(combined.IsSuccess);
        Assert.Empty(combined.Errors);
    }
}
