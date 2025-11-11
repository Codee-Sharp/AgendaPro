using System;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore.Update;

namespace AgendaPro.Domain.Shared
{
    public class Result
    {
        public bool IsSuccess { get; }
        public IReadOnlyList<Error> Errors { get; }

        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, IEnumerable<Error> errors)
        {
            IsSuccess = isSuccess;
            Errors = new ReadOnlyCollection<Error>(errors.ToList());
        }

        public static Result Success() => new(true, Array.Empty<Error>());

        public static Result Failure(params Error[] errors) => new(false, errors);

        public static Result Combine(params Result[] results)
        {
            var errors = results.SelectMany(r => r.Errors).ToArray();
            return errors.Any() ? Failure(errors) : Success();
        }
    }
}
