using AgendaPro.Domain.Tags.Models;
using AgendaPro.Domain.Tags.Repositories;
using AgendaPro.Infrastucture.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaPro.Infrastucture.Tags
{
    public class TagRepository : ITagRepository
    {
        private readonly AgendaProDbContext _context;

        public TagRepository(AgendaProDbContext context)
        {
            _context = context;
        }

        public Task SaveAsync(TagModel model)
        {
            _context.Tags.Add(model);
            return _context.SaveChangesAsync();
        }

        public async Task<TagModel?> GetByIdAsync(Guid id)
        {
            return await _context.Tags.FindAsync(id);
        }

        public async Task<IEnumerable<TagModel>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public Task UpdateAsync(TagModel model)
        {
            _context.Tags.Update(model);
            return _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var tagToDelete = await _context.Tags.FindAsync(id);
            _context.Tags.Remove(tagToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TagModel>> FilterByNameLike(string name)
        {
            return await _context.Tags
                .Where(tag => tag.Name.StartsWith(name))
                .ToListAsync();
        }
    }
}
