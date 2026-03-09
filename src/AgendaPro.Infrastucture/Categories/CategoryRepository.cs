using AgendaPro.Domain.Services.Models;
using AgendaPro.Domain.Services.Repositories;
using AgendaPro.Infrastucture.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AgendaPro.Infrastucture.Categories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AgendaProDbContext _context;
    public CategoryRepository(AgendaProDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Add(CategoryModel model)
    {
        _context.Categories.Add(model);
    }

    public void Delete(CategoryModel model)
    {
        _context.Remove(model);
    }

    public async Task<IEnumerable<CategoryModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Categories.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<CategoryModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public void Update(CategoryModel model)
    {
        _context.Categories.Update(model);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<CategoryModel?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Name == name, cancellationToken);

    }
}
