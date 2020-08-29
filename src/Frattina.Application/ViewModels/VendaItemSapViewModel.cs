using System;
using System.ComponentModel;

namespace Frattina.Application.ViewModels
{
    public class VendaItemSapViewModel
    {
        [DisplayName("Linha")]
        public int NumeroLinha { get; set; }

        [DisplayName("Código do Produto")]
        public string ProdutoId { get; set; }

        [DisplayName("Descrição do Produto")]
        public string ProdutoDescricao { get; set; }

        public decimal Quantidade { get; set; }

        [DisplayName("Valor Total")]
        public decimal ValorTotal { get; set; }

        public string Moeda { get; set; }

        [DisplayName("Desconto")]
        public decimal DescontoPercent { get; set; }

        [DisplayName("Estoque")]
        public string EstoqueCodigo { get; set; }

        [DisplayName("CFOP")]
        public string Cfop { get; set; }

        [DisplayName("Valor Unitário")]
        public decimal ValorUnitario { get; set; }

        public int VendaId { get; set; }

        [DisplayName("NCM")]
        public int Ncm { get; set; }

        public string CodigoImposto { get; set; }

        public int UtilizacaoId { get; set; }

        public Guid AtendimentoProdutoId { get; set; }
    }
}
