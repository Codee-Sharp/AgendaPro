using AgendaPro.Domain.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaPro.Application.Services.DTOs
{
    internal class ServiceDTO
    {
        public Guid? Id { get; set; }
        public string Nome { get; set; }
        public int DuracaoMin { get; set; }
        public decimal Preco { get; set; }
        public string? Descricao { get; set; }
        public int? CategoriaId { get; set; }
        public int? IntervaloMin { get; set; }


        public ServiceDTO()
        {
        }

        public ServiceDTO(ServiceModel model)
        {
            Id = model.Id;
            Nome = model.Nome;
            DuracaoMin = model.DuracaoMin;
            Preco = model.Preco;
            Descricao = model.Descricao;
            CategoriaId = model.CategoriaId;
            IntervaloMin = model.IntervaloMin;
        }

    }
}
