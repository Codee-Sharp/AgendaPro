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

        public async Task<Result<TagDto>> GetByIdAsync(Guid id)
        {
            var findTagById = await tagRepository.GetByIdAsync(id);

            if (findTagById == null)
            {
                return Result<TagDto>.Failure(new Error("NotFound", "Tag não encontrada"));
            }

            return Result<TagDto>.Success(new TagDto(findTagById));
        }

        public async Task<Result<IEnumerable<TagDto>>> GetAllAsync()
        {
            var tags = await tagRepository.GetAllAsync();
            var response = tags.Select(tag => new TagDto(tag));

            return Result<IEnumerable<TagDto>>.Success(response);
        }

        public async Task<Result<bool>> UpdateAsync(Guid id, TagDto tagDto)
        {
            if (string.IsNullOrWhiteSpace(tagDto.Name))
            {
                return Result<bool>.Failure(new Error("TAG001: Nome da tag não foi informado", "O nome da tag é obrigatório"));
            }

            var tagToUpdate = await tagRepository.GetByIdAsync(id);

            if (tagToUpdate == null)
            {
                return Result<bool>.Failure(new Error("NotFound", "Tag não encontrada"));
            }

            tagToUpdate.UpdateName(tagDto.Name);
            await tagRepository.UpdateAsync(tagToUpdate);

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            var tagToDelete = await tagRepository.GetByIdAsync(id);

            if (tagToDelete == null)
            {
                return Result<bool>.Failure(new Error("NotFound", "Tag não encontrada"));
            }

            await tagRepository.DeleteAsync(id);

            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<TagDto>>> FilterByNameLike(string name)
        {
            var tagsByName = await tagRepository.FilterByNameLike(name);
            var response = tagsByName.Select(tag => new TagDto(tag));

            return Result<IEnumerable<TagDto>>.Success(response);
        }
    }
}
