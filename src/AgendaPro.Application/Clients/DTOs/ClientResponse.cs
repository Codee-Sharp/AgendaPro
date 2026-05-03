using AgendaPro.Domain.Clients.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgendaPro.Application.Clients.DTOs
{
    public class ClientResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? Observations { get; set; }

        public ClientResponse(ClientModel model)
        {
            Id = model.Id;
            Name = model.Name;
            Email = model.Email;
            Telephone = model.Telephone;
            Observations = model.Observations;
        }
    }
}
