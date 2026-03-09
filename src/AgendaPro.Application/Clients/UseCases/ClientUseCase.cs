using AgendaPro.Application.Clients.DTOs;
using AgendaPro.Domain.Clients.Models;
using AgendaPro.Domain.Clients.Repositories;
using AgendaPro.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AgendaPro.Application.Clients.UseCases
{
    public class ClientUseCase
    {

        private readonly IClientRepository _clientRepository;

        public ClientUseCase(IClientRepository clientRepository)
        {

               _clientRepository = clientRepository;
        
        }

        ///////////////// FALTA CORRIGIR IMPLEMENTAÇÃO DE RESULT PATTERN
        public async Task<Result<ClientResponse>> CreateAsync(CreateClientRequest request)
        {
            var clientId = Guid.Empty;
            var model = new ClientModel
            (
                request.Name,
                request.Email,
                request.Telephone,
                request.Observations,
                clientId
            );
            await _clientRepository.SaveAsync(model);
            var response = new ClientResponse(model);
            return Result<ClientResponse>.Success(response);
        }

        ///////////////// FALTA CORRIGIR IMPLEMENTAÇÃO DE RESULT PATTERN

        public async Task<Result<ClientResponse>> GetByIdAsync(Guid id)
        {

            var findClientById = await _clientRepository.GetByIdAsync(id);

            if (findClientById == null)
                throw new KeyNotFoundException("Cliente não encontrado");

            return Result<ClientResponse>.Success(new ClientResponse(findClientById));
        }

        ///////////////// FALTA CORRIGIR IMPLEMENTAÇÃO DE RESULT PATTERN

        public async Task<Result<IEnumerable<ClientResponse>>>GetAllAsync()
        {
            var clients = await _clientRepository.GetAllAsync();
            var response = clients.Select(client => new ClientResponse(client));
            return Result<IEnumerable<ClientResponse>>.Success(response);
        }

        ///////////////// FALTA CORRIGIR IMPLEMENTAÇÃO DE RESULT PATTERN

        public async Task<Result<bool>> UpdateAsync(Guid id, UpdateClientRequest request)
        {
            var clientToUpdate = await _clientRepository.GetByIdAsync(id);
            if (clientToUpdate == null)
                throw new KeyNotFoundException("Cliente não encontrado");

            clientToUpdate.Update(
                request.Name,
                request.Email,
                request.Telephone,
                request.Observations
            );
            await _clientRepository.UpdateAsync(clientToUpdate);
            return Result<bool>.Success(true);
        }

        ///////////////// FALTA CORRIGIR IMPLEMENTAÇÃO DE RESULT PATTERN
        public async Task<Result<bool>> DeleteAsync(Guid id)
        {

            var clientToDelete = await _clientRepository.GetByIdAsync(id);

            if(clientToDelete == null)
                throw new KeyNotFoundException("Cliente não encontrado");

            await _clientRepository.DeleteAsync(id);

            return Result<bool>.Success(true);

        }

        public async Task<Result<IEnumerable<ClientModel>>> FilterByNameLike(string name)
        {
            var clientByName = await _clientRepository.FilterByNameLike(name);
            return Result<IEnumerable<ClientModel>>.Success(clientByName);
        }

        public async Task<Result<IEnumerable<ClientModel>>> FilterByEmailLike(string email)
        {
            var clientByEmail = await _clientRepository.FilterByEmailLike(email);
            return Result<IEnumerable<ClientModel>>.Success(clientByEmail);
        }


    }
}
