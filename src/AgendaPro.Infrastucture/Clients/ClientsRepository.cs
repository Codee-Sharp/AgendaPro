using System;
using System.Collections.Generic;
using System.Text;
using AgendaPro.Domain.Clients.Models;
using AgendaPro.Domain.Clients.Repositories;
using AgendaPro.Infrastucture.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AgendaPro.Infrastucture.Clients
{
    public class ClientsRepository : IClientRepository
    {
        // dependency injection
        private readonly AgendaProDbContext _context;

        // constructor
        public ClientsRepository(AgendaProDbContext context)
        {

            _context = context;
        
        }

        // Method for saving a client
        public async Task SaveAsync(ClientModel model)
        {

            _context.Clients.Add(model);

            await _context.SaveChangesAsync();

        }

        // Method for getting all clients
        public async Task<IEnumerable<ClientModel>> GetAllAsync()
        {

            return await _context.Clients.ToListAsync();
            
        }

        public async Task<ClientModel?> GetByIdAsync(Guid id)
        {

            return await _context.Clients.FindAsync(id);
            
        }

        public async Task UpdateAsync(ClientModel model)
        {

            _context.Clients.Update(model);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(Guid id)
        {

            var clientToDelete = _context.Clients.Find(id);

            _context.Clients.Remove(clientToDelete);

            await _context.SaveChangesAsync();

        }

    }
}
