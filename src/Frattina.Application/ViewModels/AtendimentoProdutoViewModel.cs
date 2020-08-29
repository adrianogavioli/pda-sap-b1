using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frattina.Application.ViewModels
{
    public class AtendimentoProdutoViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Código do Produto")]
        public string ProdutoSapId { get; set; }

        public string Tipo { get; set; }

        public string Marca { get; set; }

        public string Modelo { get; set; }

        [DisplayName("Referência")]
        public string Referencia { get; set; }

        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        public string Imagem { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(1, double.MaxValue, ErrorMessage ="O campo {0} não deve ser 0")]
        [DisplayName("Valor de Tabela")]
        public decimal? ValorTabela { get; set; }

        [DisplayName("Desconto")]
        public decimal? PercentDesconto { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(1, double.MaxValue, ErrorMessage = "O campo {0} não deve ser 0")]
        [DisplayName("Valor Negociado")]
        public decimal? ValorNegociado { get; set; }

        [DisplayName("Nível de Interesse")]
        public int? NivelInteresse { get; set; }

        [DisplayName("Removido A")]
        public bool RemovidoAtendimento { get; set; }

        [DisplayName("Removido V")]
        public bool RemovidoVenda { get; set; }

        public string Notificacao { get; set; }

        public Guid AtendimentoId { get; set; }

        public AtendimentoViewModel Atendimento { get; set; }

        public ProdutoSapViewModel ProdutoSap { get; set; }
    }
}
