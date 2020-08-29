using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frattina.Application.ViewModels
{
    public class AtendimentoVendidoViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage ="O campo {0} deve não deve ser menor que {1}")]
        [DisplayName("Código da Venda")]
        public int VendaCodigo { get; set; }

        [DisplayName("Data da Venda")]
        public DateTime Data { get; set; }

        public AtendimentoViewModel Atendimento { get; set; }
    }
}
