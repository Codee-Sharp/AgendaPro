using AgendaPro.Domain.Services.Models;
using AgendaPro.Domain.Services.Repositories;
using AgendaPro.Infrastucture.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaPro.Infrastucture.Services
{
    public class ServiceRepository : IServiceRepository
    {

        private readonly AgendaProDbContext _context;

        public ServiceRepository(AgendaProDbContext context)
        {
            
            _context = context;
        
        }

        // OK

        public Task SaveAsync(ServiceModel model)
        {
        
            return Task.CompletedTask;
        
        }

        // OK

        public async Task<ServiceModel?> GetByIdAsync(Guid id)
        {

            return await _context.Services.FindAsync(id);

        }

        // OK
        public async Task<IEnumerable<ServiceModel>> GetAllAsync()
        {
        
            return await _context.Services.ToListAsync();

        }

        // OK
        public async Task UpdateAsync(ServiceModel model)
    {
            _context.Services.Update(model);
            await _context.SaveChangesAsync();
    }


        // OK
        public async Task DeleteAsync(Guid id)
        {

            var serviceToDelete = await _context.Services.FindAsync(id);
            _context.Services.Remove(serviceToDelete);
            await _context.SaveChangesAsync();

        }


    }
}
