using AgendaPro.Application.Services.DTOs;
using AgendaPro.Domain.Services.Models;
using AgendaPro.Domain.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaPro.Application.Services.UseCases
{
    public class ServiceUseCase
    {

        private readonly IServiceRepository _serviceRepository;
        
        public async Task<ServiceDTO> CreateAsync(ServiceDTO serviceDTO)
        {
            var userId = Guid.Empty;

            var model = new ServiceModel(serviceDTO.Nome, userId);

            await _serviceRepository.SaveAsync(model);

            var response = new ServiceDTO(model);
            return response;
        }
    }
}
