using AgendaPro.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AgendaPro.Domain.Clients.Models
{
    public class ClientModel : AuditableEntity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string? Email { get; private set; }
        public string? Telephone { get; private set; }
        public string? Observations { get; private set; }



        public ClientModel(string name, string? email, string? telephone, string? observations, Guid createdBy)
            : base(createdBy)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome é obrigatório.", nameof(name));

            Name = name;
            Email = email;
            Telephone = telephone;
            Observations = observations;
        }



        public void Update(string name, string? email, string? telephone, string? observations)
        {
            Name = name;
            Email = email;
            Telephone = telephone;
            Observations = observations;
        }


    }
}
