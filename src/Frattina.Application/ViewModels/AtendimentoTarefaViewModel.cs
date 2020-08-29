using Frattina.Business.Models.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frattina.Application.ViewModels
{
    public class AtendimentoTarefaViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [EnumDataType(typeof(TarefaTipo), ErrorMessage = "O campo {0} é obrigatório")]
        public TarefaTipo Tipo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(500, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 3)]
        [DisplayName("Tarefa")]
        public string Assunto { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Data da Tarefa")]
        public DateTime? DataTarefa { get; set; }

        [DisplayName( "Data da Finalização")]
        public DateTime? DataFinalizacao { get; set; }

        public bool Removida { get; set; }

        public Guid AtendimentoId { get; set; }

        public AtendimentoViewModel Atendimento { get; set; }
    }
}
