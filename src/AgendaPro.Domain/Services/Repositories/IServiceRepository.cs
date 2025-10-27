using AgendaPro.Domain.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaPro.Domain.Services.Repositories
{
    public interface IServiceRepository
    {
        Task SaveAsync(ServiceModel model);
        Task<ServiceModel> GetByIdAsync(Guid id);
        Task<IEnumerable<ServiceModel>> GetAllAsync();
        Task DeleteAsync(Guid id);
        Task UpdateAsync(ServiceModel model);
    }
}
