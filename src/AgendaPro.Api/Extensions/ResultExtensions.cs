using AgendaPro.Api.Wrappers;
using AgendaPro.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace AgendaPro.Api.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(new ApiResponse<T?>(result.Value));

            var errors = result.Errors.Select(error => error.Message).ToList();
            var response = new ApiResponse<T?>(errors);

            return new BadRequestObjectResult(response);
        }
    }
}
