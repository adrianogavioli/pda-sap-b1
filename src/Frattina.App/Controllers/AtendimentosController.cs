using Frattina.App.Extensions;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.Business.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.App.Controllers
{
    [Authorize]
    public class AtendimentosController : BaseController
    {
        private readonly IAtendimentoApplication _atendimentoApplication;
        private readonly IUsuarioApplication _usuarioApplication;
        private readonly IVendedorApplication _vendedorApplication;
        private readonly IProdutoApplication _produtoApplication;
        private readonly IAuditoriaApplication _auditoriaApplication;

        public AtendimentosController(
            IAtendimentoApplication atendimentoApplication,
            IUsuarioApplication usuarioApplication,
            IVendedorApplication vendedorApplication,
            IProdutoApplication produtoApplication,
            IAuditoriaApplication auditoriaApplication,
            INotificador notificador) : base(notificador)
        {
            _atendimentoApplication = atendimentoApplication;
            _usuarioApplication = usuarioApplication;
            _vendedorApplication = vendedorApplication;
            _produtoApplication = produtoApplication;
            _auditoriaApplication = auditoriaApplication;
        }

        [ClaimsAuthorize("atendimento", "n1")]
        [Route("atendimentos")]
        public async Task<IActionResult> Index()
        {
            var atendimentosViewModel = new List<AtendimentoViewModel>();

            var usuarioViewModel = await _usuarioApplication.ObterUsuarioAuth();

            if (usuarioViewModel == null) return View(atendimentosViewModel);

            if (usuarioViewModel.Tipo == UsuarioTipo.ADMINISTRADOR)
            {
                atendimentosViewModel = await _atendimentoApplication.ObterAtendimentosProdutosTarefas();
            }
            else if (usuarioViewModel.Tipo == UsuarioTipo.GERENTE)
            {
                foreach (var empresa in usuarioViewModel.Empresas)
                {
                    atendimentosViewModel.AddRange(await _atendimentoApplication.ObterAtendimentosProdutosTarefasPorEmpresa(empresa.EmpresaId));
                }
            }
            else if (usuarioViewModel.Tipo == UsuarioTipo.VENDEDOR && usuarioViewModel.VendedorSapId != null)
            {
                atendimentosViewModel = await _atendimentoApplication.ObterAtendimentosProdutosTarefasPorVendedor((int)usuarioViewModel.VendedorSapId);
            }

            return View(atendimentosViewModel);
        }

        [ClaimsAuthorize("atendimento", "n1")]
        [Route("atendimento/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var atendimentoViewModel = await _atendimentoApplication.ObterAtendimentoProdutosTarefas(id);

            if (atendimentoViewModel == null) return NotFound();

            return View(atendimentoViewModel);
        }

        [ClaimsAuthorize("atendimento", "n2")]
        [Route("adicionar-atendimento")]
        public async Task<IActionResult> Create()
        {
            var usuarioViewModel = await _usuarioApplication.ObterUsuarioAuth();

            if (usuarioViewModel == null) return NotFound();

            var atendimentoViewModel = new AtendimentoViewModel();

            await PopularEmpresasDropdown(atendimentoViewModel, usuarioViewModel);

            await PopularVendedoresDropdown(atendimentoViewModel);

            if (atendimentoViewModel.EmpresasDropdown?.Count == 1)
            {
                atendimentoViewModel.EmpresaId = atendimentoViewModel.EmpresasDropdown[0].Id;
            }

            if (usuarioViewModel.VendedorSapId != null)
            {
                var vendedorSelect = atendimentoViewModel.VendedoresDropdown.FirstOrDefault(v => v.Id == usuarioViewModel.VendedorSapId);

                if (vendedorSelect != null)
                    atendimentoViewModel.VendedorId = vendedorSelect.Id;
            }

            return View(atendimentoViewModel);
        }

        [ClaimsAuthorize("atendimento", "n2")]
        [Route("adicionar-atendimento")]
        [HttpPost]
        public async Task<IActionResult> Create(AtendimentoViewModel atendimentoViewModel)
        {
            var usuarioViewModel = await _usuarioApplication.ObterUsuarioAuth();

            if (usuarioViewModel == null) return NotFound();

            await PopularEmpresasDropdown(atendimentoViewModel, usuarioViewModel);

            await PopularVendedoresDropdown(atendimentoViewModel);

            ModelState.Remove("Etapa");

            if (!ModelState.IsValid) return View(atendimentoViewModel);

            var atendimentoViewModelResult = await _atendimentoApplication.Adicionar(atendimentoViewModel);

            if (!OperacaoValida()) return View(atendimentoViewModel);

            TempData["Sucesso"] = "O novo atendimento foi adicionado.";

            return RedirectToAction("Edit", new { id = atendimentoViewModelResult.Id });
        }

        [ClaimsAuthorize("atendimento", "n2")]
        [Route("editar-atendimento/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var atendimentoViewModel = await _atendimentoApplication.ObterAtendimentoProdutosTarefas(id);

            if (atendimentoViewModel == null) return NotFound();

            if (atendimentoViewModel.Etapa != AtendimentoEtapa.Andamento)
            {
                TempData["Erro"] = @"Que coisa feia!\nTente outra estratégia, esta falhou. kkk";

                return RedirectToAction("Index");
            }

            await PopularVendedoresDropdown(atendimentoViewModel);

            await PopularProdutosAmados(atendimentoViewModel);

            return View(atendimentoViewModel);
        }

        [ClaimsAuthorize("atendimento", "n2")]
        [Route("editar-atendimento/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, AtendimentoViewModel atendimentoViewModel)
        {
            if (id != atendimentoViewModel.Id) return NotFound();

            var atendimentoViewModelDb = await _atendimentoApplication.ObterAtendimentoProdutosTarefas(id);

            if (atendimentoViewModelDb == null) return NotFound();

            atendimentoViewModel.Produtos = atendimentoViewModelDb.Produtos;
            atendimentoViewModel.Tarefas = atendimentoViewModelDb.Tarefas;

            await PopularVendedoresDropdown(atendimentoViewModel);

            ModelState.Remove("EmpresaId");

            if (!ModelState.IsValid) return View(atendimentoViewModel);

            await _atendimentoApplication.Atualizar(atendimentoViewModel);

            if (!OperacaoValida()) return View(atendimentoViewModel);

            TempData["Sucesso"] = "O atendimento foi atualizado.";

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("atendimento", "n2")]
        [Route("encerrar-atendimento/{id:guid}")]
        public async Task<IActionResult> Encerrar(Guid id)
        {
            var atendimentoViewModel = await _atendimentoApplication.ObterAtendimento(id);

            if (atendimentoViewModel == null) return NotFound();

            if (atendimentoViewModel.Etapa != AtendimentoEtapa.Andamento)
            {
                TempData["Erro"] = @"Que coisa feia!\nTente outra estratégia, esta falhou. kkk";
                return RedirectToAction("Index");
            }

            atendimentoViewModel.Encerrado = new AtendimentoEncerradoViewModel();

            return View(atendimentoViewModel);
        }

        [ClaimsAuthorize("atendimento", "n2")]
        [Route("encerrar-atendimento/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Encerrar(Guid id, AtendimentoViewModel atendimentoViewModel)
        {
            if (id != atendimentoViewModel.Id) return NotFound();

            var atendimentoViewModelReturn = await _atendimentoApplication.ObterAtendimento(id);

            if (atendimentoViewModelReturn == null) return NotFound();

            atendimentoViewModelReturn.Encerrado = new AtendimentoEncerradoViewModel
            {
                Motivo = atendimentoViewModel.Encerrado.Motivo,
                Justificativa = atendimentoViewModel.Encerrado.Justificativa
            };

            ModelState.Remove("EmpresaId");
            ModelState.Remove("VendedorId");
            ModelState.Remove("ClienteNome");
            ModelState.Remove("TipoPessoa");
            ModelState.Remove("Etapa");

            if (!ModelState.IsValid) return View(atendimentoViewModelReturn);

            await _atendimentoApplication.Encerrar(atendimentoViewModel);

            if (!OperacaoValida()) return View(atendimentoViewModelReturn);

            TempData["Sucesso"] = "O atendimento foi encerrado.";

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("atendimento", "n3")]
        [Route("remover-atendimento/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var atendimentoViewModel = await _atendimentoApplication.ObterAtendimento(id);

            if (atendimentoViewModel == null) return NotFound();

            if (atendimentoViewModel.Etapa != AtendimentoEtapa.Andamento)
            {
                TempData["Erro"] = @"Que coisa feia!\nTente outra estratégia, esta falhou. kkk";
                return RedirectToAction("Index");
            }

            return View(atendimentoViewModel);
        }

        [ClaimsAuthorize("atendimento", "n3")]
        [Route("remover-atendimento/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _atendimentoApplication.Remover(id);

            if (!OperacaoValida())
            {
                var atendimentoViewModel = await _atendimentoApplication.ObterAtendimento(id);

                if (atendimentoViewModel == null) return NotFound();

                return View(atendimentoViewModel);
            }

            TempData["Sucesso"] = "O atendimento foi removido.";

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("atendimento", "n2")]
        public async Task<IActionResult> AdicionarProduto(Guid atendimentoId, string produtoSapId)
        {
            var atendimentoProdutoViewModel = new AtendimentoProdutoViewModel
            {
                AtendimentoId = atendimentoId,
                ProdutoSapId = produtoSapId
            };

            if (!string.IsNullOrEmpty(produtoSapId))
            {
                var produtoSapViewModel = await _produtoApplication.ObterProduto(produtoSapId);

                if (produtoSapViewModel == null)
                    ModelState.AddModelError(string.Empty, "O código do produto é inválido.");
                else
                {
                    atendimentoProdutoViewModel.ProdutoSap = produtoSapViewModel;
                    atendimentoProdutoViewModel.ValorTabela = produtoSapViewModel.Precos.Max(p => p.Valor);
                    atendimentoProdutoViewModel.PercentDesconto = 0;
                    atendimentoProdutoViewModel.ValorNegociado = atendimentoProdutoViewModel.ValorTabela;
                }
            }

            return PartialView("_AdicionarProduto", atendimentoProdutoViewModel);
        }

        [ClaimsAuthorize("atendimento", "n2")]
        [HttpPost]
        public async Task<IActionResult> AdicionarProduto(AtendimentoProdutoViewModel atendimentoProdutoViewModel)
        {
            atendimentoProdutoViewModel.ProdutoSap = await _produtoApplication.ObterProduto(atendimentoProdutoViewModel.ProdutoSapId);

            if (!ModelState.IsValid) return PartialView("_AdicionarProduto", atendimentoProdutoViewModel);

            await _atendimentoApplication.AdicionarProduto(atendimentoProdutoViewModel);

            if (!OperacaoValida()) return PartialView("_AdicionarProduto", atendimentoProdutoViewModel);

            var url = Url.Action("ObterAtendimentoProdutos", new { atendimentoId = atendimentoProdutoViewModel.AtendimentoId });

            return Json(new { success = true, url });
        }

        [ClaimsAuthorize("atendimento", "n1")]
        [Route("detalhar-produto-atendimento/{id:guid}")]
        public async Task<IActionResult> DetalharProduto(Guid id)
        {
            var atendimentoProdutoViewModel = await _atendimentoApplication.ObterAtendimentoProduto(id);

            if (atendimentoProdutoViewModel == null) return NotFound();

            atendimentoProdutoViewModel.ProdutoSap = await _produtoApplication.ObterProduto(atendimentoProdutoViewModel.ProdutoSapId);

            return PartialView("_DetalharProduto", atendimentoProdutoViewModel);
        }

        [ClaimsAuthorize("atendimento", "n2")]
        [Route("editar-produto-atendimento/{id:guid}")]
        public async Task<IActionResult> EditarProduto(Guid id)
        {
            var atendimentoProdutoViewModel = await _atendimentoApplication.ObterAtendimentoProduto(id);

            if (atendimentoProdutoViewModel == null) return NotFound();

            atendimentoProdutoViewModel.ProdutoSap = await _produtoApplication.ObterProduto(atendimentoProdutoViewModel.ProdutoSapId);

            return PartialView("_EditarProduto", atendimentoProdutoViewModel);
        }

        [ClaimsAuthorize("atendimento", "n2")]
        [Route("editar-produto-atendimento/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> EditarProduto(Guid id, AtendimentoProdutoViewModel atendimentoProdutoViewModel)
        {
            if (id != atendimentoProdutoViewModel.Id) return NotFound();

            atendimentoProdutoViewModel.ProdutoSap = await _produtoApplication.ObterProduto(atendimentoProdutoViewModel.ProdutoSapId);

            ModelState.Remove("ProdutoSapId");

            if (!ModelState.IsValid) return PartialView("_EditarProduto", atendimentoProdutoViewModel);

            await _atendimentoApplication.AtualizarProdutoValorNegociado(atendimentoProdutoViewModel);

            if (!OperacaoValida()) return PartialView("_EditarProduto", atendimentoProdutoViewModel);

            var url = Url.Action("ObterAtendimentoProdutos", new { atendimentoId = atendimentoProdutoViewModel.AtendimentoId });

            return Json(new { success = true, url });
        }

        [ClaimsAuthorize("atendimento", "n3")]
        [Route("remover-produto-atendimento/{id:guid}")]
        public async Task<IActionResult> RemoverProduto(Guid id)
        {
            var atendimentoProdutoViewModel = await _atendimentoApplication.ObterAtendimentoProduto(id);

            if (atendimentoProdutoViewModel == null) return NotFound();

            atendimentoProdutoViewModel.ProdutoSap = await _produtoApplication.ObterProduto(atendimentoProdutoViewModel.ProdutoSapId);

            return PartialView("_RemoverProduto", atendimentoProdutoViewModel);
        }

        [ClaimsAuthorize("atendimento", "n3")]
        [Route("remover-produto-atendimento/{id:guid}")]
        [HttpPost, ActionName("RemoverProduto")]
        public async Task<IActionResult> RemoverProdutoConfirmado(Guid id)
        {
            var atendimentoProdutoViewModel = await _atendimentoApplication.ObterAtendimentoProduto(id);

            if (atendimentoProdutoViewModel == null) return NotFound();

            await _atendimentoApplication.RemoverProdutoAtendimento(atendimentoProdutoViewModel);

            if (!OperacaoValida())
            {
                atendimentoProdutoViewModel.ProdutoSap = await _produtoApplication.ObterProduto(atendimentoProdutoViewModel.ProdutoSapId);

                return PartialView("_RemoverProduto", atendimentoProdutoViewModel);
            }

            var url = Url.Action("ObterAtendimentoProdutos", new { atendimentoId = atendimentoProdutoViewModel.AtendimentoId });

            return Json(new { success = true, url });
        }

        [ClaimsAuthorize("atendimento", "n2")]
        [Route("atualizar-produto-nivelinteresse")]
        public async Task<IActionResult> AtualizarProdutoNivelInteresse(Guid id, int nivelInteresse)
        {
            var atendimentoProdutoViewModel = await _atendimentoApplication.ObterAtendimentoProduto(id);

            if (atendimentoProdutoViewModel == null) return null;

            atendimentoProdutoViewModel.NivelInteresse = nivelInteresse;

            await _atendimentoApplication.AtualizarProdutoNivelInteresse(atendimentoProdutoViewModel);

            if (!OperacaoValida()) return null;

            var url = Url.Action("ObterAtendimentoProdutos", new { atendimentoId = atendimentoProdutoViewModel.AtendimentoId });
            return Json(new { success = true, url });
        }

        [ClaimsAuthorize("atendimento", "n2")]
        [Route("adicionar-tarefa-atendimento")]
        public IActionResult AdicionarTarefa(Guid atendimentoId)
        {
            var atendimentoTarefaViewModel = new AtendimentoTarefaViewModel
            {
                AtendimentoId = atendimentoId
            };

            return PartialView("_AdicionarTarefa", atendimentoTarefaViewModel);
        }

        [ClaimsAuthorize("atendimento", "n2")]
        [Route("adicionar-tarefa-atendimento")]
        [HttpPost]
        public async Task<IActionResult> AdicionarTarefa(AtendimentoTarefaViewModel atendimentoTarefaViewModel)
        {
            if (!ModelState.IsValid) return PartialView("_AdicionarTarefa", atendimentoTarefaViewModel);

            await _atendimentoApplication.AdicionarTarefa(atendimentoTarefaViewModel);

            if (!OperacaoValida()) return PartialView("_AdicionarTarefa", atendimentoTarefaViewModel);

            var url = Url.Action("ObterAtendimentoTarefas", new { atendimentoId = atendimentoTarefaViewModel.AtendimentoId });
            return Json(new { success = true, url });
        }

        [ClaimsAuthorize("atendimento", "n1")]
        [Route("detalhar-tarefa-atendimento/{id:guid}")]
        public async Task<IActionResult> DetalharTarefa(Guid id)
        {
            var atendimentoTarefaViewModel = await _atendimentoApplication.ObterAtendimentoTarefa(id);

            if (atendimentoTarefaViewModel == null) return NotFound();

            return PartialView("_DetalharTarefa", atendimentoTarefaViewModel);
        }

        [ClaimsAuthorize("atendimento", "n2")]
        [Route("editar-tarefa-atendimento/{id:guid}")]
        public async Task<IActionResult> EditarTarefa(Guid id)
        {
            var atendimentoTarefaViewModel = await _atendimentoApplication.ObterAtendimentoTarefa(id);

            if (atendimentoTarefaViewModel == null) return NotFound();

            return PartialView("_EditarTarefa", atendimentoTarefaViewModel);
        }

        [ClaimsAuthorize("atendimento", "n2")]
        [Route("editar-tarefa-atendimento/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> EditarTarefa(Guid id, AtendimentoTarefaViewModel atendimentoTarefaViewModel)
        {
            if (id != atendimentoTarefaViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return PartialView("_EditarTarefa", atendimentoTarefaViewModel);

            await _atendimentoApplication.AtualizarTarefa(atendimentoTarefaViewModel);

            if (!OperacaoValida()) return PartialView("_EditarTarefa", atendimentoTarefaViewModel);

            var url = Url.Action("ObterAtendimentoTarefas", new { atendimentoId = atendimentoTarefaViewModel.AtendimentoId });
            return Json(new { success = true, url });
        }

        [ClaimsAuthorize("atendimento", "n2")]
        [Route("finalizar-tarefa-atendimento/{id:guid}")]
        public async Task<IActionResult> FinalizarTarefa(Guid id)
        {
            var atendimentoTarefaViewModel = await _atendimentoApplication.ObterAtendimentoTarefa(id);

            if (atendimentoTarefaViewModel == null) return NotFound();

            return PartialView("_FinalizarTarefa", atendimentoTarefaViewModel);
        }

        [ClaimsAuthorize("atendimento", "n2")]
        [Route("finalizar-tarefa-atendimento/{id:guid}")]
        [HttpPost, ActionName("FinalizarTarefa")]
        public async Task<IActionResult> FinalizarTarefaConfirmado(Guid id)
        {
            var atendimentoTarefaViewModel = await _atendimentoApplication.ObterAtendimentoTarefa(id);

            if (atendimentoTarefaViewModel == null) return NotFound();

            await _atendimentoApplication.FinalizarTarefa(atendimentoTarefaViewModel);

            if (!OperacaoValida()) return PartialView("_FinalizarTarefa", atendimentoTarefaViewModel);

            var url = Url.Action("ObterAtendimentoTarefas", new { atendimentoId = atendimentoTarefaViewModel.AtendimentoId });
            return Json(new { success = true, url });
        }

        [ClaimsAuthorize("atendimento", "n3")]
        [Route("remover-tarefa-atendimento/{id:guid}")]
        public async Task<IActionResult> RemoverTarefa(Guid id)
        {
            var atendimentoTarefaViewModel = await _atendimentoApplication.ObterAtendimentoTarefa(id);

            if (atendimentoTarefaViewModel == null) return NotFound();

            return PartialView("_RemoverTarefa", atendimentoTarefaViewModel);
        }

        [ClaimsAuthorize("atendimento", "n3")]
        [Route("remover-tarefa-atendimento/{id:guid}")]
        [HttpPost, ActionName("RemoverTarefa")]
        public async Task<IActionResult> RemoverTarefaConfirmado(Guid id)
        {
            var atendimentoTarefaViewModel = await _atendimentoApplication.ObterAtendimentoTarefa(id);

            if (atendimentoTarefaViewModel == null) return NotFound();

            await _atendimentoApplication.RemoverTarefa(atendimentoTarefaViewModel);

            if (!OperacaoValida()) return PartialView("_RemoverTarefa", atendimentoTarefaViewModel);

            var url = Url.Action("ObterAtendimentoTarefas", new { atendimentoId = atendimentoTarefaViewModel.AtendimentoId });
            return Json(new { success = true, url });
        }

        private Task<AtendimentoViewModel> PopularEmpresasDropdown(AtendimentoViewModel atendimentoViewModel, UsuarioViewModel usuarioViewModel)
        {
            var empresasSapViewModel = new List<EmpresaSapViewModel>();

            foreach (var empresa in usuarioViewModel.Empresas)
            {
                empresasSapViewModel.Add(new EmpresaSapViewModel
                {
                    Id = empresa.EmpresaId,
                    RazaoSocial = empresa.EmpresaRazaoSocial
                });
            }

            atendimentoViewModel.EmpresasDropdown = empresasSapViewModel.OrderBy(e => e.RazaoSocial).ToList();

            return Task.FromResult(atendimentoViewModel);
        }

        private async Task<AtendimentoViewModel> PopularVendedoresDropdown(AtendimentoViewModel atendimentoViewModel)
        {
            var vendedoresSapViewModel = await _vendedorApplication.ObterTodos();

            atendimentoViewModel.VendedoresDropdown = vendedoresSapViewModel.OrderBy(v => v.Nome).ToList();

            return atendimentoViewModel;
        }

        private async Task<AtendimentoViewModel> PopularProdutosAmados(AtendimentoViewModel atendimentoViewModel)
        {
            atendimentoViewModel.ProdutosAmados = await _atendimentoApplication.ObterAtendimentoProdutosAmadosPorCliente(atendimentoViewModel.ClienteId, true);

            foreach (var produto in atendimentoViewModel.Produtos)
            {
                atendimentoViewModel.ProdutosAmados.RemoveAll(p => p.ProdutoSapId == produto.ProdutoSapId);
            }

            return atendimentoViewModel;
        }

        public async Task<IActionResult> ObterAtendimentoProdutos(Guid atendimentoId)
        {
            var atendimentoProdutosViewModel = await _atendimentoApplication.ObterAtendimentoProdutosPorAtendimento(atendimentoId);

            if (atendimentoProdutosViewModel == null) return NotFound();

            return PartialView("_ListaProdutos", new AtendimentoViewModel
            {
                Produtos = atendimentoProdutosViewModel
            });
        }

        public async Task<IActionResult> ObterAtendimentoTarefas(Guid atendimentoId)
        {
            var atendimentoTarefasViewModel = await _atendimentoApplication.ObterAtendimentoTarefasPorAtendimento(atendimentoId);

            if (atendimentoTarefasViewModel == null) return NotFound();

            return PartialView("_ListaTarefas", new AtendimentoViewModel
            {
                Tarefas = atendimentoTarefasViewModel
            });
        }

        [ClaimsAuthorize("auditoria", "n1")]
        public async Task<IActionResult> Auditoria(Guid chave)
        {
            var atendimentoViewModel = await _atendimentoApplication.ObterAtendimentoProdutosTarefasAuditoria(chave);

            if (atendimentoViewModel == null) return NotFound();

            var auditoriasViewModel = await _auditoriaApplication.ObterAuditorias("Atendimento", chave);

            foreach (var item in atendimentoViewModel.Produtos)
            {
                auditoriasViewModel.AddRange(await _auditoriaApplication.ObterAuditorias("AtendimentoProduto", item.Id));
            }

            foreach (var item in atendimentoViewModel.Tarefas)
            {
                auditoriasViewModel.AddRange(await _auditoriaApplication.ObterAuditorias("AtendimentoTarefa", item.Id));
            }

            if (atendimentoViewModel.Encerrado != null)
            {
                auditoriasViewModel.AddRange(await _auditoriaApplication.ObterAuditorias("AtendimentoEncerrado", atendimentoViewModel.Encerrado.Id));
            }

            if (atendimentoViewModel.Vendido != null)
            {
                auditoriasViewModel.AddRange(await _auditoriaApplication.ObterAuditorias("AtendimentoVendido", atendimentoViewModel.Vendido.Id));
            }

            TempData["AuditoriaRouteController"] = "Atendimentos";
            TempData["AuditoriaRouteChave"] = chave.ToString();

            return PartialView("../Auditorias/_Auditoria", auditoriasViewModel.OrderBy(a => a.Data).ToList());
        }
    }
}