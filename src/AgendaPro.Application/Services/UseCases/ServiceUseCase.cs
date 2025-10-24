using AgendaPro.Application.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaPro.Application.Services.UseCases
{
    // 
    public class ServiceUseCase(IServiceRepository serviceRepository)
    {
        public async Task<ServiceDTO> CreateAsync(ServiceDTO serviceDTO)
        {
        }
    }
}
