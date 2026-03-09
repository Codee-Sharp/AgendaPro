using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AgendaPro.Application.Clients.DTOs
{
    public class UpdateClientRequest
    {
        [Required]
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? Observations { get; set; }
    }
}
