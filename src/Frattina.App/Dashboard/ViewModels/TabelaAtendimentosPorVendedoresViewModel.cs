using System.ComponentModel;

namespace Frattina.App.Dashboard.ViewModels
{
    public class TabelaAtendimentosPorVendedoresViewModel
    {
        public string Vendedor { get; set; }

        [DisplayName("Total de Atendimentos")]
        public int QtdAtendimento { get; set; }

        [DisplayName("Total de Vendas")]
        public int QtdVenda { get; set; }

        [DisplayName("Taxa de Conversão (%)")]
        public decimal TaxaConversaoVenda { get; set; }
    }
}
