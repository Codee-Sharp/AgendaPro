using AgendaPro.Application.Services.DTOs;
using AgendaPro.Domain.Services.Models;
using AgendaPro.Domain.Services.Repositories;
using AgendaPro.Infrastucture.Data.Context;
using Microsoft.EntityFrameworkCore;
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
        
        public ServiceUseCase(IServiceRepository serviceRepository)
        {

            _serviceRepository = serviceRepository;

        }


        public async Task<ServiceDTO> CreateAsync(ServiceDTO serviceDTO)
        {

            var userId = Guid.Empty;

            var model = new ServiceModel
                (
                serviceDTO.Nome,
                serviceDTO.DuracaoMin,
                serviceDTO.Preco,
                serviceDTO.Descricao,
                serviceDTO.CategoriaId,
                serviceDTO.TempoIntervaloMin,
                userId
                );

            await _serviceRepository.SaveAsync(model);

            var response = new ServiceDTO(model);

            return response;

        }


        // Implementar lógica de negócio chamando IRepository
        public async Task<ServiceModel> GetByIdAsync(Guid id)
        {

            var findOneService = await _serviceRepository.GetByIdAsync(id);

            if (findOneService == null)
            {
                throw new KeyNotFoundException("Serviço não encontrado");
            }

            return findOneService;

        }


        // Separar conexão de dados (repository) e lógica de negócio (use case)
        public async Task<IEnumerable<ServiceModel>> GetAllAsync()
        {

            return await _serviceRepository.GetAllAsync();

        }


        // Separar conexão de dados (repository) e lógica de negócio (use case)
        public async Task UpdateAsync(Guid id, ServiceDTO serviceDTO)
        {
            var serviceToUpdate = await _serviceRepository.GetByIdAsync(id);

            if (serviceToUpdate == null)
                throw new KeyNotFoundException("Serviço não encontrado");

            serviceToUpdate.Nome = serviceDTO.Nome;
            serviceToUpdate.DuracaoMin = serviceDTO.DuracaoMin;
            serviceToUpdate.Preco = serviceDTO.Preco;
            serviceToUpdate.Descricao = serviceDTO.Descricao;
            serviceToUpdate.CategoriaId = serviceDTO.CategoriaId;
            serviceToUpdate.TempoIntervaloMin = serviceDTO.TempoIntervaloMin;

            await _serviceRepository.UpdateAsync(serviceToUpdate);

        }


        // Separar conexão de dados (repository) e lógica de negócio (use case)
        public async Task DeleteAsync(Guid id)
        {

            var serviceToDelete = await GetByIdAsync(id);

            if (serviceToDelete == null)
                throw new KeyNotFoundException("Serviço não encontrado");

            await _serviceRepository.DeleteAsync(id);

        }
    }
}
