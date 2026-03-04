using AgendaPro.Api.Extensions;
using AgendaPro.Application.Tags.Dtos;
using AgendaPro.Application.Tags.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace AgendaPro.Api.Controllers
{
    [ApiController]
    [Route("api/tags")]
    public class TagController(TagUseCase tagUseCase, ILogger<TagController> logger) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TagDto tagDto)
        {
            logger.LogInformation("Creating a new tag");
            var result = await tagUseCase.CreateAsync(tagDto);
            logger.LogDebug("Operation for created executed!");

            return result.ToActionResult();
        }
    }
}
