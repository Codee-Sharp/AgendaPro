using AgendaPro.Domain.Tags.Models;

namespace AgendaPro.Domain.Tags.Repositories
{
    public interface ITagRepository
    {
        Task SaveAsync(TagModel model);

        Task<TagModel?> GetByIdAsync(Guid id);

        Task<IEnumerable<TagModel>> GetAllAsync();

        Task UpdateAsync(TagModel model);

        Task DeleteAsync(Guid id);

        Task<IEnumerable<TagModel>> FilterByNameLike(string name);
    }
}
