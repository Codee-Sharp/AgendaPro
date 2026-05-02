using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AgendaPro.Application.Clients.DTOs
{
    public class CreateClientRequest
    {
        public string  Name { get; set; }
        public string? Email { get; set; }

        [StringLength(16, MinimumLength = 8,
            ErrorMessage = "O telefone deve ter entre 8 e 15 caracteres")]
        [RegularExpression(@"^[0-9+\-\s()]{8,15}$",
            ErrorMessage = "O telefone contém caracteres inválidos")]
        public string? Telephone { get; set; }
        public string? Observations { get; set; }
    }
}
