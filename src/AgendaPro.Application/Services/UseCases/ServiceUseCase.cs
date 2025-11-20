using AgendaPro.Application.Services.DTOs;
using AgendaPro.Domain.Services.Models;
using AgendaPro.Domain.Services.Repositories;
using AgendaPro.Domain.Shared;
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


        public async Task<Result<ServiceDTO>> CreateAsync(ServiceDTO serviceDTO)
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

            return Result<ServiceDTO>.Success(response);

        }


        // Implementar lógica de negócio chamando IRepository
        public async Task<Result<ServiceModel>> GetByIdAsync(Guid id)
        {

            var findOneService = await _serviceRepository.GetByIdAsync(id);

            if (findOneService == null)
            {
                throw new KeyNotFoundException("Serviço não encontrado");
            }

            return Result<ServiceModel>.Success(findOneService);

        }


        // Separar conexão de dados (repository) e lógica de negócio (use case)
        public async Task<Result<IEnumerable<ServiceModel>>> GetAllAsync()
        {
            var servicesResult = await _serviceRepository.GetAllAsync();
            return Result<IEnumerable<ServiceModel>>.Success(servicesResult);

        }


        // Separar conexão de dados (repository) e lógica de negócio (use case)
        public async Task<Result<bool>> UpdateAsync(Guid id, ServiceDTO serviceDTO)
        {
            var serviceToUpdate = await _serviceRepository.GetByIdAsync(id);

            if (serviceToUpdate == null)
                throw new KeyNotFoundException("Serviço não encontrado");

            serviceToUpdate.UpdateService(
                serviceDTO.Nome,
                serviceDTO.DuracaoMin,
                serviceDTO.Preco,
                serviceDTO.Descricao,
                serviceDTO.CategoriaId,
                serviceDTO.TempoIntervaloMin
            );
        

            await _serviceRepository.UpdateAsync(serviceToUpdate);

            return Result<bool>.Success(true);

        }


        // Separar conexão de dados (repository) e lógica de negócio (use case)
        public async Task<Result<bool>> DeleteAsync(Guid id)
        {

            var serviceToDelete = await GetByIdAsync(id);

            if (serviceToDelete == null)
                throw new KeyNotFoundException("Serviço não encontrado");

            await _serviceRepository.DeleteAsync(id);

            return Result<bool>.Success(true);

        }
    }
}
