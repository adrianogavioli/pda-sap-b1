using Frattina.App.Extensions;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.Business.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.App.Controllers
{
    [Authorize]
    public class VendasController : BaseController
    {
        private readonly IVendaApplication _vendaApplication;
        private readonly IAtendimentoApplication _atendimentoApplication;
        private readonly IUsuarioApplication _usuarioApplication;
        private readonly IClienteApplication _clienteApplication;
        private readonly IProdutoApplication _produtoApplication;
        private readonly IEstoqueApplication _estoqueApplication;
        private readonly IProdutoEstoqueApplication _produtoEstoqueApplication;

        public VendasController(IVendaApplication vendaApplication,
            IAtendimentoApplication atendimentoApplication,
            IUsuarioApplication usuarioApplication,
            IClienteApplication clienteApplication,
            IProdutoApplication produtoApplication,
            IEstoqueApplication estoqueApplication,
            IProdutoEstoqueApplication produtoEstoqueApplication,
            INotificador notificador) : base(notificador)
        {
            _vendaApplication = vendaApplication;
            _atendimentoApplication = atendimentoApplication;
            _usuarioApplication = usuarioApplication;
            _clienteApplication = clienteApplication;
            _produtoApplication = produtoApplication;
            _estoqueApplication = estoqueApplication;
            _produtoEstoqueApplication = produtoEstoqueApplication;
        }

        [ClaimsAuthorize("venda", "n1")]
        [Route("vendas")]
        public async Task<IActionResult> Index(int? numeroNfFilter, string clienteFilter, DateTime? dataEmissaoIniFilter, DateTime? dataEmissaoFimFilter)
        {
            var usuarioViewModel = await _usuarioApplication.ObterUsuarioAuth();

            if (usuarioViewModel == null) return NotFound();

            var searchFilters = await GerarFiltrosPesquisas(numeroNfFilter, clienteFilter, dataEmissaoIniFilter, dataEmissaoFimFilter);

            SearchFiltersFactory(searchFilters);

            return View(await EfetuarConsultaVendas(usuarioViewModel, searchFilters));
        }

        [ClaimsAuthorize("venda", "n1")]
        [Route("venda/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            var vendaViewModel = await _vendaApplication.ObterVenda(id);

            if (vendaViewModel == null) return NotFound();

            return View(vendaViewModel);
        }

        [ClaimsAuthorize("venda", "n2")]
        [Route("adicionar-venda/{atendimentoid:guid}")]
        public async Task<ActionResult> Create(Guid atendimentoId)
        {
            var atendimentoViewModel = await _atendimentoApplication.ObterAtendimentoProdutosTarefas(atendimentoId);

            if (atendimentoViewModel == null) return NotFound();

            if (atendimentoViewModel.Produtos == null || atendimentoViewModel.Produtos.Count == 0)
            {
                TempData["Erro"] = "Este atendimento não possui produtos.";

                return RedirectToAction("Index", "Atendimentos");
            }

            if (atendimentoViewModel.Etapa != AtendimentoEtapa.Andamento)
            {
                TempData["Erro"] = @"Que coisa feia!\nTente outra estratégia, esta falhou. kkk";

                return RedirectToAction("Index", "Atendimentos");
            }

            var clienteViewModel = await _clienteApplication.ObterCliente(atendimentoViewModel.ClienteIdVenda);

            if (clienteViewModel == null)
            {
                TempData["Erro"] = "Não foi possível obter os dados do cliente.";

                return RedirectToAction("Index", "Atendimentos");
            }

            if (!await _vendaApplication.ClienteEstaAptoParaComprar(clienteViewModel))
            {
                TempData["Alerta"] = "Para efetivar a venda será necessário adicionar algumas informações do cliente.";
            }

            await OcultarProdutosRemovidosVenda(atendimentoViewModel.Produtos);

            return View(atendimentoViewModel);
        }

        [ClaimsAuthorize("venda", "n2")]
        [Route("adicionar-venda/{atendimentoid:guid}")]
        [HttpPost]
        public async Task<ActionResult> Create(Guid atendimentoId, AtendimentoViewModel atendimentoViewModel)
        {
            if (atendimentoId != atendimentoViewModel.Id) return NotFound();

            var atendimentoViewModelDb = await _atendimentoApplication.ObterAtendimentoProdutosTarefas(atendimentoViewModel.Id);

            await OcultarProdutosRemovidosVenda(atendimentoViewModelDb.Produtos);

            ModelState.Remove("TipoPessoa");
            ModelState.Remove("Etapa");

            if (!ModelState.IsValid) return View(atendimentoViewModelDb);

            var vendaViewModel = await PopularVenda(atendimentoViewModelDb);

            await _vendaApplication.Adicionar(vendaViewModel);

            if (!OperacaoValida()) return View(atendimentoViewModelDb);

            TempData["Sucesso"] = "A venda foi efetuada.";

            return RedirectToAction("Index", "Atendimentos");
        }

        [ClaimsAuthorize("venda", "n2")]
        [Route("alterar-cliente-venda/{id:guid}")]
        public IActionResult AlterarClienteVenda(Guid id)
        {
            return PartialView("_AlterarClienteVenda", new AtendimentoViewModel { Id = id });
        }

        [ClaimsAuthorize("venda", "n2")]
        [Route("alterar-cliente-venda/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> AlterarClienteVenda(Guid id, AtendimentoViewModel atendimentoViewModel)
        {
            if (id != atendimentoViewModel.Id) return NotFound();

            ModelState.Remove("EmpresaId");
            ModelState.Remove("VendedorId");
            ModelState.Remove("ClienteNome");
            ModelState.Remove("TipoPessoa");
            ModelState.Remove("Etapa");

            if (!ModelState.IsValid) return PartialView("_AlterarClienteVenda", atendimentoViewModel);

            await _atendimentoApplication.AtualizarClienteVenda(atendimentoViewModel);

            if (!OperacaoValida()) return PartialView("_AlterarClienteVenda", atendimentoViewModel);

            TempData["Sucesso"] = "O cliente foi substituído.";

            var url = HttpContext.Request.GetTypedHeaders().Referer.AbsoluteUri;

            return Json(new { success = true, url });
        }

        [ClaimsAuthorize("venda", "n1")]
        [Route("detalhar-produto-venda/{id:guid}")]
        public async Task<IActionResult> DetalharProduto(Guid id)
        {
            var atendimentoProdutoViewModel = await _atendimentoApplication.ObterAtendimentoProduto(id);

            if (atendimentoProdutoViewModel == null) return NotFound();

            atendimentoProdutoViewModel.ProdutoSap = await _produtoApplication.ObterProduto(atendimentoProdutoViewModel.ProdutoSapId);

            return PartialView("_DetalharProduto", atendimentoProdutoViewModel);
        }

        [ClaimsAuthorize("venda", "n2")]
        [Route("adicionar-produto-venda/{atendimentoId:guid}")]
        public async Task<IActionResult> AdicionarProduto(Guid atendimentoId)
        {
            var atendimentoProdutosViewModel = await _atendimentoApplication.ObterAtendimentoProdutosPorAtendimento(atendimentoId);

            atendimentoProdutosViewModel.RemoveAll(p => !p.RemovidoVenda);

            await NotificarDisponibilidadeEstoque(atendimentoId, atendimentoProdutosViewModel);

            return PartialView("_AdicionarProduto", atendimentoProdutosViewModel);
        }

        [ClaimsAuthorize("venda", "n2")]
        [Route("adicionar-produto-venda/{atendimentoId:guid}")]
        [HttpPost]
        public async Task<IActionResult> AdicionarProduto(Guid atendimentoId, AtendimentoProdutoViewModel atendimentoProdutoViewModel)
        {
            if (atendimentoId != atendimentoProdutoViewModel.AtendimentoId) return NotFound();

            await _atendimentoApplication.AdicionarProdutoVenda(atendimentoProdutoViewModel);

            if (!OperacaoValida())
            {
                var atendimentoProdutosViewModel = await _atendimentoApplication.ObterAtendimentoProdutosPorAtendimento(atendimentoId);

                atendimentoProdutosViewModel.RemoveAll(p => !p.RemovidoVenda);

                return PartialView("_AdicionarProduto", atendimentoProdutosViewModel);
            }

            var url = Url.Action("ObterAtendimentoProdutos", new { atendimentoId });

            return Json(new { success = true, url });
        }

        [ClaimsAuthorize("venda", "n2")]
        [Route("editar-produto-venda/{id:guid}")]
        public async Task<IActionResult> EditarProduto(Guid id)
        {
            var atendimentoProdutoViewModel = await _atendimentoApplication.ObterAtendimentoProduto(id);

            if (atendimentoProdutoViewModel == null) return NotFound();

            atendimentoProdutoViewModel.ProdutoSap = await _produtoApplication.ObterProduto(atendimentoProdutoViewModel.ProdutoSapId);

            return PartialView("_EditarProduto", atendimentoProdutoViewModel);
        }

        [ClaimsAuthorize("venda", "n2")]
        [Route("editar-produto-venda/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> EditarProduto(Guid id, AtendimentoProdutoViewModel atendimentoProdutoViewModel)
        {
            if (id != atendimentoProdutoViewModel.Id) return NotFound();

            atendimentoProdutoViewModel.ProdutoSap = await _produtoApplication.ObterProduto(atendimentoProdutoViewModel.ProdutoSapId);

            if (!ModelState.IsValid) return PartialView("_EditarProduto", atendimentoProdutoViewModel);

            await _atendimentoApplication.AtualizarProdutoValorNegociado(atendimentoProdutoViewModel);

            if (!OperacaoValida()) return PartialView("_EditarProduto", atendimentoProdutoViewModel);

            var url = Url.Action("ObterAtendimentoProdutos", new { atendimentoId = atendimentoProdutoViewModel.AtendimentoId });

            return Json(new { success = true, url });
        }

        [ClaimsAuthorize("venda", "n3")]
        [Route("remover-produto-venda/{id:guid}")]
        public async Task<IActionResult> RemoverProduto(Guid id)
        {
            var atendimentoProdutoViewModel = await _atendimentoApplication.ObterAtendimentoProduto(id);

            if (atendimentoProdutoViewModel == null) return NotFound();

            atendimentoProdutoViewModel.ProdutoSap = await _produtoApplication.ObterProduto(atendimentoProdutoViewModel.ProdutoSapId);

            return PartialView("_RemoverProduto", atendimentoProdutoViewModel);
        }

        [ClaimsAuthorize("venda", "n3")]
        [Route("remover-produto-venda/{id:guid}")]
        [HttpPost, ActionName("RemoverProduto")]
        public async Task<IActionResult> RemoverProdutoConfirmado(Guid id)
        {
            var atendimentoProdutoViewModel = await _atendimentoApplication.ObterAtendimentoProduto(id);

            if (atendimentoProdutoViewModel == null) return NotFound();

            atendimentoProdutoViewModel.ProdutoSap = await _produtoApplication.ObterProduto(atendimentoProdutoViewModel.ProdutoSapId);

            await _atendimentoApplication.RemoverProdutoVenda(atendimentoProdutoViewModel);

            if (!OperacaoValida()) return PartialView("_RemoverProduto", atendimentoProdutoViewModel);

            var url = Url.Action("ObterAtendimentoProdutos", new { atendimentoId = atendimentoProdutoViewModel.AtendimentoId });

            return Json(new { success = true, url });
        }

        private async Task<List<VendaSapViewModel>> EfetuarConsultaVendas(UsuarioViewModel usuarioViewModel, List<SearchFilter> searchFilters)
        {
            var vendasViewModel = new List<VendaSapViewModel>();

            var numeroNfFilter = searchFilters[0];
            var clienteFilter = searchFilters[1];
            var dataEmissaoIniFilter = searchFilters[2];
            var dataEmissaoFimFilter = searchFilters[3];

            if (numeroNfFilter?.ValueFilter != null)
            {
                var vendaViewModel = await _vendaApplication.ObterPorNumeroNf((int)numeroNfFilter.ValueFilter);

                if (vendaViewModel != null)
                    vendasViewModel.Add(vendaViewModel);
            }
            else if (clienteFilter?.ValueFilter != null)
            {
                vendasViewModel.AddRange(await _vendaApplication.ObterPorCliente((string)clienteFilter.ValueFilter));
            }
            else if (dataEmissaoIniFilter?.ValueFilter != null || dataEmissaoFimFilter?.ValueFilter != null)
            {
                if (dataEmissaoIniFilter?.ValueFilter == null) dataEmissaoIniFilter = new SearchFilter { KeyFilter = "", ValueFilter = DateTime.MinValue.Date };

                if (dataEmissaoFimFilter?.ValueFilter == null) dataEmissaoFimFilter = new SearchFilter { KeyFilter = "", ValueFilter = DateTime.Today};

                vendasViewModel.AddRange(await _vendaApplication.ObterPorData((DateTime)dataEmissaoIniFilter.ValueFilter, (DateTime)dataEmissaoFimFilter.ValueFilter));
            }
            else
            {
                if (usuarioViewModel.Tipo == UsuarioTipo.ADMINISTRADOR || usuarioViewModel.Tipo == UsuarioTipo.GERENTE)
                {
                    var dataIni = DateTime.Today.AddMonths(-1);
                    var dataFim = DateTime.Today;

                    vendasViewModel.AddRange(await _vendaApplication.ObterPorData(dataIni, dataFim));
                }
                else if (usuarioViewModel.Tipo == UsuarioTipo.VENDEDOR && usuarioViewModel.VendedorSapId != null)
                {
                    vendasViewModel.AddRange(await _vendaApplication.ObterPorVendedor((int)usuarioViewModel.VendedorSapId));
                }
            }

            if (usuarioViewModel.Tipo == UsuarioTipo.VENDEDOR && usuarioViewModel.VendedorSapId != null)
            {
                vendasViewModel.RemoveAll(v => v.VendedorId != usuarioViewModel.VendedorSapId);
            }

            return vendasViewModel;
        }

        public async Task<IActionResult> ObterAtendimentoProdutos(Guid atendimentoId)
        {
            var atendimentoProdutosViewModel = await _atendimentoApplication.ObterAtendimentoProdutosPorAtendimento(atendimentoId);

            if (atendimentoProdutosViewModel == null) return NotFound();

            await OcultarProdutosRemovidosVenda(atendimentoProdutosViewModel);

            return PartialView("_ListaProdutos", new AtendimentoViewModel
            {
                Produtos = atendimentoProdutosViewModel
            });
        }

        private async Task<VendaSapViewModel> PopularVenda(AtendimentoViewModel atendimentoViewModel)
        {
            return new VendaSapViewModel
            {
                DataEmissao = DateTime.Today,
                EmpresaId = atendimentoViewModel.EmpresaId,
                ClienteId = atendimentoViewModel.ClienteIdVenda,
                ClienteNome = atendimentoViewModel.ClienteNomeVenda,
                ValorTotal = atendimentoViewModel.Produtos.Sum(p => Convert.ToDecimal(p.ValorNegociado)),
                HoraEmissao = DateTime.Now.TimeOfDay.ToString(),
                VendedorId = atendimentoViewModel.VendedorId,
                AtendimentoId = atendimentoViewModel.Id,
                Vendedor = new VendedorSapViewModel
                {
                    Id = atendimentoViewModel.VendedorId,
                    Nome = atendimentoViewModel.VendedorNome
                },
                Produtos = await PopularVendaItens(atendimentoViewModel.Produtos)
            };
        }

        private Task<List<VendaItemSapViewModel>> PopularVendaItens(List<AtendimentoProdutoViewModel> atendimentoProdutosViewModel)
        {
            var vendaItensViewModel = new List<VendaItemSapViewModel>();

            foreach (var item in atendimentoProdutosViewModel.Where(p => !p.RemovidoAtendimento && !p.RemovidoVenda))
            {
                vendaItensViewModel.Add(new VendaItemSapViewModel
                {
                    NumeroLinha = atendimentoProdutosViewModel.IndexOf(item) + 1,
                    ProdutoId = item.ProdutoSapId,
                    ProdutoDescricao = item.Descricao,
                    Quantidade = 1,
                    ValorTotal = Convert.ToDecimal(item.ValorNegociado),
                    ValorUnitario = Convert.ToDecimal(item.ValorNegociado),
                    AtendimentoProdutoId = item.Id
                });
            }

            return Task.FromResult(vendaItensViewModel);
        }

        private Task<List<AtendimentoProdutoViewModel>> OcultarProdutosRemovidosVenda(List<AtendimentoProdutoViewModel> atendimentoProdutosViewModel)
        {
            atendimentoProdutosViewModel.RemoveAll(p => p.RemovidoVenda);

            return Task.FromResult(atendimentoProdutosViewModel);
        }

        private async Task<List<AtendimentoProdutoViewModel>> NotificarDisponibilidadeEstoque(Guid atendimentoId, List<AtendimentoProdutoViewModel> atendimentoProdutosViewModel)
        {
            var atendimentoViewModel = await _atendimentoApplication.ObterAtendimento(atendimentoId);
            
            if (atendimentoViewModel == null) return atendimentoProdutosViewModel;

            var estoquesViewModel = await _estoqueApplication.ObterPorEmpresa(atendimentoViewModel.EmpresaId);

            if (estoquesViewModel.Count == 0) return atendimentoProdutosViewModel;

            foreach (var item in atendimentoProdutosViewModel)
            {
                var possuiDisponibilidadeEstoque = false;

                var produtoEstoquesViewModel = await _produtoEstoqueApplication.ObterPorProduto(item.ProdutoSapId);

                foreach (var itemEstoque in produtoEstoquesViewModel.Where(e => e.Quantidade > 0))
                {
                    if (estoquesViewModel.Any(e => e.PermiteVenda && e.Codigo == itemEstoque.Estoque))
                        possuiDisponibilidadeEstoque = true;
                }

                if (!possuiDisponibilidadeEstoque)
                    item.Notificacao = "indisponível";
            }

            return atendimentoProdutosViewModel;
        }

        public async Task<ActionResult> LimparFiltrosPesquisa(int? numeroNfFilter, string clienteFilter, DateTime? dataEmissaoIniFilter, DateTime? dataEmissaoFimFilter)
        {
            CleanSearchFilters(await GerarFiltrosPesquisas(numeroNfFilter, clienteFilter, dataEmissaoIniFilter, dataEmissaoFimFilter));

            return RedirectToAction("Index");
        }

        private Task<List<SearchFilter>> GerarFiltrosPesquisas(int? numeroNfFilter, string clienteFilter, DateTime? dataEmissaoIniFilter, DateTime? dataEmissaoFimFilter)
        {
            return Task.FromResult(new List<SearchFilter>
            {
                new SearchFilter{ KeyFilter = "VendaNumeroNfFilter", ValueFilter = numeroNfFilter },
                new SearchFilter{ KeyFilter = "VendaClienteFilter", ValueFilter = clienteFilter },
                new SearchFilter{ KeyFilter = "VendaDataEmissaoIniFilter", ValueFilter = dataEmissaoIniFilter },
                new SearchFilter{ KeyFilter = "VendaDataEmissaoFimFilter", ValueFilter = dataEmissaoFimFilter }
            });
        }
    }
}