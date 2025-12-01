using AgendaPro.Domain.Clients.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgendaPro.Domain.Clients.Repositories
{
    public interface IClientRepository
    {

        Task SaveAsync(ClientModel model);

        Task<ClientModel?> GetByIdAsync(Guid id);

        Task<IEnumerable<ClientModel>> GetAllAsync();

        Task UpdateAsync(ClientModel model);

        Task DeleteAsync(Guid id);

    }
}
