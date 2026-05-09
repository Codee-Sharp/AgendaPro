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

        // get by id
        Task<ServiceModel?> GetByIdAsync(Guid id);

        // list all 
        Task<IEnumerable<ServiceModel>> GetAllAsync();

        // update
        Task UpdateAsync(ServiceModel model);

        // delete
        Task DeleteAsync(Guid id);

        Task<IEnumerable<ServiceModel>> FilterByNameLike(string name);

        Task<IEnumerable<ServiceModel>> FilterByDescriptionLike(string description);

    }
}
