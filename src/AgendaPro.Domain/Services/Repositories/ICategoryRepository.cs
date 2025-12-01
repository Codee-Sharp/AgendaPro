using AgendaPro.Domain.Services.Models;

namespace AgendaPro.Domain.Services.Repositories;

public interface ICategoryRepository
{
   Task<IEnumerable<CategoryModel>> GetAllAsync(CancellationToken cancellationToken);
    Task<CategoryModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<CategoryModel?> GetByNameAsync(string name, CancellationToken cancellationToken);
    void Add(CategoryModel model);
    void Update(CategoryModel model);
    void Delete(CategoryModel model);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
