using AgendaPro.Domain.Clients.Models;
using AgendaPro.Domain.Clients.Repositories;
using AgendaPro.Infrastucture.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AgendaPro.Infrastucture.Clients
{
    public class ClientsRepository : IClientRepository
    {
        private readonly AgendaProDbContext _context;



        public ClientsRepository(AgendaProDbContext context)
        {
            _context = context;
        }



        public async Task SaveAsync(ClientModel model)
        {
            _context.Clients.Add(model);
            await _context.SaveChangesAsync();
        }



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



        public async Task<IEnumerable<ClientModel>> FilterByNameLike(string name)
        {
            return await _context.Clients
                .Where(c => c.Name.StartsWith(name)).ToListAsync();
        }



        public async Task<IEnumerable<ClientModel>> FilterByEmailLike(string email)
        {
            // buscar email : 
            // Flexibilidade: Se você buscar por @gmail.com, o Contains retornará todos os clientes que usam esse provedor
            // Facilidade de Busca: Muitos usuários buscam pelo sobrenome ou pelo domínio. Se o e - mail for joao.silva @empresa.com, buscar por silva funciona
            // Padrão de UX: Em Dashboards Administrativos (onde funcionários buscam clientes), o comportamento esperado é que qualquer parte do texto digitada traga resultados

            return await _context.Clients
                .Where(c => c.Email != null && c.Email.Contains(email)).ToListAsync();
        }

    }
}
