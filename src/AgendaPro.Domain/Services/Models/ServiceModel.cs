using AgendaPro.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            TempoIntervaloMin = intervaloInt;
        }


        public Guid Id { get; set; }

        // obrigatorio
        [Required]
        public string Nome { get; set; }

        // obrigatorio + em minutos
        [Required]
        [Range(1,480, ErrorMessage = "A duração deve estar entre 1 e 480 minutos")] // entre 1 min e 8 horas
        public int DuracaoMin { get; set; }

        // obrigatorio
        [Required]
        public decimal Preco { get; set; }


        public string? Descricao { get; set; }

        public int? CategoriaId { get; set; }

        public int? TempoIntervaloMin { get; set; } = 0;
    
        public void UpdateService(string nome, int duracaoMin, decimal preco, string? descricao, int? categoriaId, int? tempoIntervaloMin)
        {

            Nome = nome;
            DuracaoMin = duracaoMin;
            Preco = preco;
            Descricao = descricao;
            CategoriaId = categoriaId;
            TempoIntervaloMin = tempoIntervaloMin;

        }

    }
}