using AgendaPro.Application.Tags.Dtos;
using AgendaPro.Domain.Shared;
using AgendaPro.Domain.Tags.Models;
using AgendaPro.Domain.Tags.Repositories;

namespace AgendaPro.Application.Tags.UseCase
{
    public class TagUseCase(ITagRepository tagRepository)
    {
        public async Task<Result<TagDto>> CreateAsync(TagDto tagDto)
        {
            if (string.IsNullOrWhiteSpace(tagDto.Name))
            {
                return Result<TagDto>.Failure(new Error("TAG001: Nome da tag não foi informado", "O nome da tag é obrigatório"));
            }

            var userId = Guid.Empty;
            var model = new TagModel(tagDto.Name, userId);

            await tagRepository.SaveAsync(model);

            var reponse = new TagDto(model);

            return Result<TagDto>.Success(reponse);
        }
    }
}
