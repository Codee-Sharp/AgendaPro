using AgendaPro.Domain.Tags.Models;

namespace AgendaPro.Domain.Tags.Repositories
{
    public interface ITagRepository
    {
        Task SaveAsync(TagModel model);
    }
}
