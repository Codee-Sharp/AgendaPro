using AgendaPro.Application.Tags.Dtos;
using AgendaPro.Domain.Tags.Models;
using AgendaPro.Domain.Tags.Repositories;

namespace AgendaPro.Application.Tags.UseCase
{
    public class TagUseCase(ITagRepository tagRepository)
    {
        public async Task<TagDto> CreateAsync(TagDto tagDto)
        {
            var userId = Guid.Empty;
            var model = new TagModel(tagDto.Name, userId);


            await tagRepository.SaveAsync(model);


            var reponse = new TagDto(model);

            return reponse;
        }
    }
}
