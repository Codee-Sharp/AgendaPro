using AgendaPro.Domain.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaPro.Application.Services.DTOs
{
    public class ServiceDTO
    {

        public Guid? Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [Range(1, 480, ErrorMessage = "A duração deve estar entre 1 e 480 minutos")] // entre 1 min e 8 horas
        public int DuracaoMin { get; set; }

        [Required]
        public decimal Preco { get; set; }

        public string? Descricao { get; set; }

        public int? CategoriaId { get; set; }

        public int? TempoIntervaloMin { get; set; } = 0;

        public ServiceDTO() { }

        public ServiceDTO(ServiceModel model)
        {

            Id = model.Id;
            Nome = model.Nome;
            DuracaoMin = model.DuracaoMin;
            Preco = model.Preco;
            Descricao = model.Descricao;
            CategoriaId = model.CategoriaId;
            TempoIntervaloMin = model.TempoIntervaloMin;

        }
    }
}
