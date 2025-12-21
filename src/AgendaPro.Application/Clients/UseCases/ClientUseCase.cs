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
    internal class ClientUseCase
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
                clienId
            );

            await _clientRepository.SaveAsync(model);

            var response = new ClientDTO(model);

            return Result<ClientDTO>.Success(response);

        }


    }
}
