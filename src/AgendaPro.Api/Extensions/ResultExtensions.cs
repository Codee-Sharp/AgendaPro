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
            {
                var responseSuccess = new ApiResponse<T?>(result.Value);
                return new OkObjectResult(responseSuccess);
            }

            if (result.Errors[0].Code == "NotFound")
            {
                return new NotFoundObjectResult(new ApiResponse<T?>(result.Errors.Select(e => e.Message).ToList()));
            }

            var errors = result.Errors.Select(e => e.Message);
            var responseFail = new ApiResponse<T?>([.. errors]);
            return new BadRequestObjectResult(responseFail);
        }
    }
}
