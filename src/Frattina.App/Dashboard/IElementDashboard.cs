using Frattina.App.Dashboard.ViewModels;
using Frattina.Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.App.Dashboard
{
    public interface IElementDashboard
    {
        Task<GraficoTaxaConversaoVendaViewModel> ObterGraficoTaxaConversaoVenda();

        Task<List<TabelaAtendimentosPorVendedoresViewModel>> ObterTabelaAtendimentosPorVendedores();

        Task<List<AtendimentoTarefaViewModel>> ObterTabelaTarefasFuturas(UsuarioViewModel usuarioViewModel);

        Task<List<GaleriaTopDezProdutosAmadosViewModel>> ObterGaleriaTopDezProdutosAmados();

        Task<List<GaleriaTopDezProdutosNaoAmadosViewModel>> ObterGaleriaTopDezProdutosNaoAmados();

        Task<int> ObterCartaoTarefasAtrasadas(UsuarioViewModel usuarioViewModel);
    }
}
