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
    internal class ServiceRepository : IServiceRepository
    {

        private readonly AgendaProDbContext _context;

        public ServiceRepository(AgendaProDbContext context)
        {
            
            _context = context;
        
        }


        public Task SaveAsync(ServiceModel model)
        {
        
            return Task.CompletedTask;
        
        }


        public async Task<IEnumerable<ServiceModel>> GetAllAsync()
        {
            
            var getAll = await _context.Services.ToListAsync();

            return getAll;
        
        }


        public async Task<ServiceModel> GetByIdAsync(Guid id)
        {
        
            var findOneService = await _context.Services.FindAsync(id);

            if (findOneService == null)
            {
                throw new KeyNotFoundException("Serviço não encontrado");
            }

            return findOneService;
        }


        public async Task UpdateAsync(ServiceModel model)
        {
            
            var serviceToUpdate = await _context.Services.FindAsync(model.Id);
           
            if (serviceToUpdate == null)
            {
                throw new KeyNotFoundException("Serviço não encontrado");
            }

            serviceToUpdate.Nome = model.Nome;
            serviceToUpdate.DuracaoMin = model.DuracaoMin;
            serviceToUpdate.Preco = model.Preco;
            serviceToUpdate.Descricao = model.Descricao;
            serviceToUpdate.CategoriaId = model.CategoriaId;
            serviceToUpdate.IntervaloMin = model.IntervaloMin;

            _context.Services.Update(serviceToUpdate);
            
            await _context.SaveChangesAsync();
        
        }


        public async Task DeleteAsync(Guid id)
        {
        
            var serviceToDelete = await _context.Services.FindAsync(id);
            
            if (serviceToDelete == null)
            {
                throw new KeyNotFoundException("Serviço não encontrado");
            }

            _context.Services.Remove(serviceToDelete);
            
            await _context.SaveChangesAsync();
        
        }


    }
}
