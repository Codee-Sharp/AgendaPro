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
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            logger.LogInformation("Listing tags");
            var result = await tagUseCase.GetAllAsync();

            return result.ToActionResult();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            logger.LogInformation("Getting tag by id");
            var result = await tagUseCase.GetByIdAsync(id);

            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TagDto tagDto)
        {
            logger.LogInformation("Creating a new tag");
            var result = await tagUseCase.CreateAsync(tagDto);
            logger.LogDebug("Operation for created executed!");

            return result.ToActionResult();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] TagDto tagDto)
        {
            logger.LogInformation("Updating tag");
            var result = await tagUseCase.UpdateAsync(id, tagDto);

            return result.ToActionResult();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            logger.LogInformation("Deleting tag");
            var result = await tagUseCase.DeleteAsync(id);

            return result.ToActionResult();
        }

        [HttpGet("filter-by-name")]
        public async Task<IActionResult> FilterByNameLike([FromQuery] string name)
        {
            logger.LogInformation("Filtering tags by name");
            var result = await tagUseCase.FilterByNameLike(name);

            return result.ToActionResult();
        }
    }
}
