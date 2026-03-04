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

            var errors = result.Errors.Select(e => e.Message);
            var responseFail = new ApiResponse<T?>([.. errors]);
            return new BadRequestObjectResult(responseFail);
        }
    }
}
