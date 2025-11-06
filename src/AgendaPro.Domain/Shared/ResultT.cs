using System;

namespace AgendaPro.Domain.Shared;

public class Result<T> : Result
{
  public T? Value { get; }

    private Result(T? value) : base(true, Array.Empty<Error>())
    {
        Value = value;
    }

    public Result(T? value, IEnumerable<Error> errors) : base(false, errors)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(value);

    public static new Result<T> Failure(params Error[] errors)
        => new(default, errors);
        
    public T GetValueOrThrow()
    {
        if (IsFailure)
            throw new InvalidOperationException("Cannot access Value on a failed result.");
        return Value!;
    }
}
