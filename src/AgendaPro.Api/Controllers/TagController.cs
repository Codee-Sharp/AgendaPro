using AgendaPro.Api.Wrappers;
using AgendaPro.Application.Tags.Dtos;
using AgendaPro.Application.Tags.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace AgendaPro.Api.Controllers
{
    [ApiController]
    [Route("api/tags")]
    public class TagController(TagUseCase tagUseCase, ILogger<TagController> logger ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TagDto tagDto)
        {
            logger.LogInformation("Creating a new tag");
            var result = await tagUseCase.CreateAsync(tagDto);

            if (result.IsFailure)
            {
                var errorMessages = result.Errors.Select(e => e.Message).ToList();
                return BadRequest(new ApiResponse<TagDto>(errorMessages));
            }

            logger.LogDebug("Created new tag");

            var responde = new ApiResponse<TagDto?>(result.Value);
            return Ok(responde);
        }
    }
}
