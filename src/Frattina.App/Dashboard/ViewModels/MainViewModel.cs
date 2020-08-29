using Frattina.Application.ViewModels;
using System.Collections.Generic;

namespace Frattina.App.Dashboard.ViewModels
{
    public class MainViewModel
    {
        public GraficoTaxaConversaoVendaViewModel GraficoTaxaConversaoVenda { get; set; }

        public List<TabelaAtendimentosPorVendedoresViewModel> TabelaAtendimentosPorVendedores { get; set; }

        public List<AtendimentoTarefaViewModel> TabelaTarefasFuturas { get; set; }

        public List<GaleriaTopDezProdutosAmadosViewModel> GaleriaTopDezProdutosAmados { get; set; }

        public List<GaleriaTopDezProdutosNaoAmadosViewModel> GaleriaTopDezProdutosNaoAmados { get; set; }

        public int CartaoTarefasAtrasadas { get; set; }
    }
}
