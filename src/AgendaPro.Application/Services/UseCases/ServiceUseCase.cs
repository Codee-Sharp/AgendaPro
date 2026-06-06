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


        public async Task<Result<ServiceDto>> CreateAsync(ServiceDto ServiceDto)
        {

            var userId = Guid.Empty;

            var model = new ServiceModel
                (
                ServiceDto.Nome,
                ServiceDto.DuracaoMin,
                ServiceDto.Preco,
                ServiceDto.Descricao,
                ServiceDto.CategoriaId,
                ServiceDto.TempoIntervaloMin,
                userId
                );

            await _serviceRepository.SaveAsync(model);

            var response = new ServiceDto(model);

            return Result<ServiceDto>.Success(response);

        }


        // Implementar lógica de negócio chamando IRepository
        public async Task<Result<ServiceModel>> GetByIdAsync(Guid id)
        {

            var findOneService = await _serviceRepository.GetByIdAsync(id);

            if (findOneService == null)
            {
                return Result<ServiceModel>.Failure(new Error("NotFound", "Serviço não encontrado"));
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
        public async Task<Result<bool>> UpdateAsync(Guid id, ServiceDto ServiceDto)
        {
            var serviceToUpdate = await _serviceRepository.GetByIdAsync(id);

            if (serviceToUpdate == null)
                return Result<bool>.Failure(new Error("NotFound", "Serviço não encontrado"));

            serviceToUpdate.UpdateService(
                ServiceDto.Nome,
                ServiceDto.DuracaoMin,
                ServiceDto.Preco,
                ServiceDto.Descricao,
                ServiceDto.CategoriaId,
                ServiceDto.TempoIntervaloMin
            );
        

            await _serviceRepository.UpdateAsync(serviceToUpdate);

            return Result<bool>.Success(true);

        }


        // Separar conexão de dados (repository) e lógica de negócio (use case)
        public async Task<Result<bool>> DeleteAsync(Guid id)
        {

            var serviceToDelete = await _serviceRepository.GetByIdAsync(id);

            if (serviceToDelete == null)
                return Result<bool>.Failure(new Error("NotFound", "Serviço não encontrado"));

            await _serviceRepository.DeleteAsync(id);

            return Result<bool>.Success(true);

        }

        public async Task<Result<IEnumerable<ServiceModel>>> FilterByNameLike(string name)
        {
            var serviceByName = await _serviceRepository.FilterByNameLike(name);
            return Result<IEnumerable<ServiceModel>>.Success(serviceByName);
        }

        public async Task<Result<IEnumerable<ServiceModel>>> FilterByDescriptionLike(string description)
        {
            var serviceByDescription = await _serviceRepository.FilterByDescriptionLike(description);
            return Result<IEnumerable<ServiceModel>>.Success(serviceByDescription);
        }
    }
}
