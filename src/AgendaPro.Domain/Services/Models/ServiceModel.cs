using AgendaPro.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaPro.Domain.Services.Models
{
    public class ServiceModel : AuditableEntity 
    {

        public ServiceModel(string nome, Guid createdBy)
            : base(createdBy)
        {
            Nome = nome;
        }


        public ServiceModel(
            string nome,
            int duracaoMin,
            decimal preco,
            string? descricao,
            int? categoriaInt,
            int? intervaloInt,
            Guid createdBy) 
            : base(createdBy)
        {
            Nome = nome;
            DuracaoMin = duracaoMin;
            Preco = preco;
            Descricao = descricao;
            CategoriaId = categoriaInt;
            IntervaloMin = intervaloInt;
        }


        public Guid Id { get; set; }

        // obrigatorio
        public string Nome { get; set; }

        // obrigatorio + em minutos
        public int DuracaoMin { get; set; }

        // obrigatorio
        public decimal Preco { get; set; }
        public string? Descricao { get; set; }
        public int? CategoriaId { get; set; }
        public int? IntervaloMin { get; set; } = 0;
    

        public void UpdateService(string nome, int duracaoMin, decimal preco, string? descricao, int? categoriaId, int? intervaloMin)
        {
            Nome = nome;
            DuracaoMin = duracaoMin;
            Preco = preco;
            Descricao = descricao;
            CategoriaId = categoriaId;
            IntervaloMin = intervaloMin;
        }

    }
}