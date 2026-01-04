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


        public async Task<Result<ClientDTO>> CreateAsync(ClientDTO clientDTO)
        {

            var clientId = Guid.Empty;

            var model = new ClientModel
            (
                clientDTO.Name,
                clientDTO.Email,
                clientDTO.Telephone,
                clientDTO.Observations,
                clientId
            );

            await _clientRepository.SaveAsync(model);

            var response = new ClientDTO(model);

            return Result<ClientDTO>.Success(response);

        }


        public async Task<Result<ClientModel>> GetByIdAsync(Guid id)
        {

            var findOneClient = await _clientRepository.GetByIdAsync(id);

            if (findOneClient == null)
            {
                throw new KeyNotFoundException("Cliente não encontrado");
            }

            return Result<ClientModel>.Success(findOneClient);
        }


        public async Task<Result<IEnumerable<ClientModel>>> GetAllAsync()
        {

            var clients = await _clientRepository.GetAllAsync();

            return Result<IEnumerable<ClientModel>>.Success(clients);

        }


        public async Task<Result<bool>> UpdateAsync(Guid id, ClientDTO clientDTO)
        {

            var clientToUpdate = await _clientRepository.GetByIdAsync(id);

            if (clientToUpdate == null)
                throw new KeyNotFoundException("Cliente não encontrado");

            clientToUpdate.Update(
                clientDTO.Name,
                clientDTO.Email,
                clientDTO.Telephone,
                clientDTO.Observations
            );

            await _clientRepository.UpdateAsync(clientToUpdate);

            return Result<bool>.Success(true);

        }


        public async Task<Result<bool>> DeleteAsync(Guid id)
        {

            var clientToDelete = await GetByIdAsync(id);

            if(clientToDelete == null)
                throw new KeyNotFoundException("Cliente não encontrado");

            await _clientRepository.DeleteAsync(id);

            return Result<bool>.Success(true);

        }


    }
}
