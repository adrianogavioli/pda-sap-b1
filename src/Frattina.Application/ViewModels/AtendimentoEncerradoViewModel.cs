using Frattina.Business.Models.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frattina.Application.ViewModels
{
    public class AtendimentoEncerradoViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public AtendimentoEncerradoMotivo Motivo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(1000, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 10)]
        public string Justificativa { get; set; }

        [DisplayName("Data do Encerramento")]
        public DateTime Data { get; set; }

        public AtendimentoViewModel Atendimento { get; set; }
    }
}
