using AgendaPro.Domain.Clients.Models;
using AgendaPro.Domain.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AgendaPro.Application.Clients.DTOs
{
    public class ClientDTO
    {
        public const int minimumCharactersForNumber = 8;
        
        public const int maximumCharactersForNumber = 16;

        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Email { get; set; }

        [StringLength(maximumCharactersForNumber, MinimumLength = minimumCharactersForNumber, 
            ErrorMessage = "O telefone deve ter entre 8 e 15 caracteres")]
        [RegularExpression(@"^[0-9+\-\s()]{8,15}$", 
            ErrorMessage = "O telefone contém caracteres inválidos.")]
        public string? Telephone { get; set; }

        public string? Observations { get; set; }


        public ClientDTO() { }

        public ClientDTO(ClientModel model)
        {

            Id = model.Id;
            Name = model.Name;
            Email = model.Email;
            Telephone = model.Telephone;
            Observations = model.Observations;

        }

    }
}
