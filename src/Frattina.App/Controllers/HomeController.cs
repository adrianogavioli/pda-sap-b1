using Frattina.App.Dashboard;
using Frattina.App.Dashboard.ViewModels;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.Business.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Frattina.App.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IUsuarioApplication _usuarioApplication;
        private readonly IElementDashboard _elementDashboard;

        public HomeController(
            IUsuarioApplication usuarioApplication,
            IElementDashboard elementDashboard,
            INotificador notificador) : base(notificador)
        {
            _usuarioApplication = usuarioApplication;
            _elementDashboard = elementDashboard;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioViewModel = await _usuarioApplication.ObterUsuarioAuth();

            if (usuarioViewModel == null) return NotFound();

            var mainViewModel = new MainViewModel();

            switch (usuarioViewModel.Tipo)
            {
                case UsuarioTipo.ADMINISTRADOR:
                    mainViewModel.TabelaAtendimentosPorVendedores = await _elementDashboard.ObterTabelaAtendimentosPorVendedores();
                    mainViewModel.GraficoTaxaConversaoVenda = await _elementDashboard.ObterGraficoTaxaConversaoVenda();
                    mainViewModel.TabelaTarefasFuturas = await _elementDashboard.ObterTabelaTarefasFuturas(usuarioViewModel);
                    mainViewModel.CartaoTarefasAtrasadas = await _elementDashboard.ObterCartaoTarefasAtrasadas(usuarioViewModel);
                    mainViewModel.GaleriaTopDezProdutosAmados = await _elementDashboard.ObterGaleriaTopDezProdutosAmados();
                    mainViewModel.GaleriaTopDezProdutosNaoAmados = await _elementDashboard.ObterGaleriaTopDezProdutosNaoAmados();
                    break;
                case UsuarioTipo.GERENTE:
                    mainViewModel.TabelaAtendimentosPorVendedores = await _elementDashboard.ObterTabelaAtendimentosPorVendedores();
                    mainViewModel.GraficoTaxaConversaoVenda = await _elementDashboard.ObterGraficoTaxaConversaoVenda();
                    mainViewModel.TabelaTarefasFuturas = await _elementDashboard.ObterTabelaTarefasFuturas(usuarioViewModel);
                    mainViewModel.CartaoTarefasAtrasadas = await _elementDashboard.ObterCartaoTarefasAtrasadas(usuarioViewModel);
                    mainViewModel.GaleriaTopDezProdutosAmados = await _elementDashboard.ObterGaleriaTopDezProdutosAmados();
                    mainViewModel.GaleriaTopDezProdutosNaoAmados = await _elementDashboard.ObterGaleriaTopDezProdutosNaoAmados();
                    break;
                case UsuarioTipo.VENDEDOR:
                    mainViewModel.TabelaTarefasFuturas = await _elementDashboard.ObterTabelaTarefasFuturas(usuarioViewModel);
                    mainViewModel.CartaoTarefasAtrasadas = await _elementDashboard.ObterCartaoTarefasAtrasadas(usuarioViewModel);
                    break;
                default:
                    break;
            }

            return View(mainViewModel);
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Errors(int id)
        {
            var modelErro = new ErrorViewModel();

            if (id == 500)
            {
                modelErro.ErroCode = id;
                modelErro.Titulo = "Ocorreu um erro";
                modelErro.SubTitulo = "olha! por essa eu não esperava :(";
                modelErro.Mensagem = "Tente novamente ou contate nosso suporte.";
            }
            else if (id == 404)
            {
                modelErro.ErroCode = id;
                modelErro.Titulo = "Página não encontrada";
                modelErro.SubTitulo = "algo não está certo!";
                modelErro.Mensagem = "A página que está procurando não existe. <br />Em caso de dúvidas entre em contato com nosso suporte.";
            }
            else if (id == 403)
            {
                modelErro.ErroCode = id;
                modelErro.Titulo = "Acesso Negado";
                modelErro.SubTitulo = "te peguei tentando aprontar, heim!";
                modelErro.Mensagem = "Você não tem permissão para acessar esse recurso, vou contar tudo para o chefe!";
            }
            else
            {
                return StatusCode(500);
            }

            return View("Error", modelErro);
        }
    }
}
