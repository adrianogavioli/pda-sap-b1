using Frattina.App.Extensions;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.App.Controllers
{
    [Authorize]
    public class UsuariosController : BaseController
    {
        private readonly IUsuarioApplication _usuarioApplication;
        private readonly ICargoApplication _cargoApplication;
        private readonly IVendedorApplication _vendedorApplication;
        private readonly IEmpresaApplication _empresaApplication;
        private readonly IAuditoriaApplication _auditoriaApplication;
        private readonly IRelUsuarioEmpresaApplication _relUsuarioEmpresaApplication;

        public UsuariosController(
            IUsuarioApplication usuarioApplication,
            ICargoApplication cargoApplication,
            IVendedorApplication vendedorApplication,
            IEmpresaApplication empresaApplication,
            IAuditoriaApplication auditoriaApplication,
            IRelUsuarioEmpresaApplication relUsuarioEmpresaApplication,
            INotificador notificador) : base(notificador)
        {
            _usuarioApplication = usuarioApplication;
            _cargoApplication = cargoApplication;
            _vendedorApplication = vendedorApplication;
            _empresaApplication = empresaApplication;
            _auditoriaApplication = auditoriaApplication;
            _relUsuarioEmpresaApplication = relUsuarioEmpresaApplication;
        }

        [ClaimsAuthorize("usuario", "n1")]
        [Route("usuarios")]
        public async Task<IActionResult> Index()
        {
            var usuariosViewModel = await _usuarioApplication.ObterUsuarios();

            return View(usuariosViewModel.OrderBy(u => u.Nome).ToList());
        }

        [ClaimsAuthorize("usuario", "n1")]
        [Route("usuario/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var usuarioViewModel = await _usuarioApplication.ObterUsuarioClaims(id);

            if (usuarioViewModel == null) return NotFound();

            await PopularEmpresasDropdown(usuarioViewModel);
            await SelecionarPermissoesEmpresasDropdown(usuarioViewModel);

            return View(usuarioViewModel);
        }

        [ClaimsAuthorize("usuario", "n2")]
        [Route("adicionar-usuario")]
        public async Task<IActionResult> Create()
        {
            var usuarioViewModel = new UsuarioViewModel();

            await PopularCargosDropdown(usuarioViewModel);
            await PopularUsuariosSapDropdown(usuarioViewModel);
            await PopularVendedoresSapDropdown(usuarioViewModel);

            return View(usuarioViewModel);
        }

        [ClaimsAuthorize("usuario", "n2")]
        [Route("adicionar-usuario")]
        [HttpPost]
        public async Task<IActionResult> Create(UsuarioViewModel usuarioViewModel)
        {
            await PopularCargosDropdown(usuarioViewModel);
            await PopularUsuariosSapDropdown(usuarioViewModel);
            await PopularVendedoresSapDropdown(usuarioViewModel);

            if (!ModelState.IsValid) return View(usuarioViewModel);

            var usuarioViewModelReturn = await _usuarioApplication.Adicionar(usuarioViewModel);

            if (!OperacaoValida()) return View(usuarioViewModel);

            TempData["Sucesso"] = "O novo usuário foi adicionado.";

            return RedirectToAction("Permissoes", new { id = usuarioViewModelReturn.Id });
        }

        [ClaimsAuthorize("usuario", "n2")]
        [Route("editar-usuario/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var usuarioViewModel = await _usuarioApplication.ObterUsuarioClaims(id);

            if (usuarioViewModel == null) return NotFound();

            await PopularCargosDropdown(usuarioViewModel);
            await PopularUsuariosSapDropdown(usuarioViewModel);
            await PopularVendedoresSapDropdown(usuarioViewModel);
            await PopularEmpresasDropdown(usuarioViewModel);
            await SelecionarPermissoesEmpresasDropdown(usuarioViewModel);

            return View(usuarioViewModel);
        }

        [ClaimsAuthorize("usuario", "n2")]
        [Route("editar-usuario/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, UsuarioViewModel usuarioViewModel)
        {
            if (id != usuarioViewModel.Id) return NotFound();

            var usuarioDb = await _usuarioApplication.ObterUsuarioClaims(id);

            if (usuarioDb == null) return NotFound();

            await PopularCargosDropdown(usuarioViewModel);
            await PopularUsuariosSapDropdown(usuarioViewModel);
            await PopularVendedoresSapDropdown(usuarioViewModel);
            await PopularEmpresasDropdown(usuarioViewModel);

            usuarioViewModel.Empresas = usuarioDb.Empresas;

            await SelecionarPermissoesEmpresasDropdown(usuarioViewModel);

            usuarioViewModel.Claims = usuarioDb.Claims;

            ModelState.Remove("Senha");

            if (!ModelState.IsValid) return View(usuarioViewModel);

            await _usuarioApplication.Atualizar(usuarioViewModel);

            if (!OperacaoValida()) return View(usuarioViewModel);

            TempData["Sucesso"] = "Os dados do usuário foram atualizados.";

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("usuario", "n3")]
        [Route("desabilitar-usuario/{id:guid}")]
        public async Task<IActionResult> Disable(Guid id)
        {
            var usuarioViewModel = await _usuarioApplication.ObterUsuarioClaims(id);

            if (usuarioViewModel == null) return NotFound();

            return View(usuarioViewModel);
        }

        [ClaimsAuthorize("usuario", "n3")]
        [Route("desabilitar-usuario/{id:guid}")]
        [HttpPost, ActionName("Disable")]
        public async Task<IActionResult> DisableConfirmed(Guid id)
        {
            await _usuarioApplication.Bloquear(id);

            if (!OperacaoValida()) return View(await _usuarioApplication.ObterUsuarioClaims(id));

            TempData["Sucesso"] = "O acesso do usuário foi bloqueado.";

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("usuario", "n3")]
        [Route("habilitar-usuario/{id:guid}")]
        public async Task<IActionResult> Enable(Guid id)
        {
            var usuarioViewModel = await _usuarioApplication.ObterUsuarioClaims(id);

            if (usuarioViewModel == null) return NotFound();

            return View(usuarioViewModel);
        }

        [ClaimsAuthorize("usuario", "n3")]
        [Route("habilitar-usuario/{id:guid}")]
        [HttpPost, ActionName("Enable")]
        public async Task<IActionResult> EnableConfirmed(Guid id)
        {
            await _usuarioApplication.Desbloquear(id);

            if (!OperacaoValida()) return View(await _usuarioApplication.ObterUsuarioClaims(id));

            TempData["Sucesso"] = "O acesso do usuário foi desbloqueado.";

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("usuario", "n2")]
        [Route("usuario-permissoes/{id:guid}")]
        public async Task<IActionResult> Permissoes(Guid id)
        {
            var usuarioViewModel = await _usuarioApplication.ObterUsuarioClaims(id);

            if (usuarioViewModel == null) return NotFound();

            await PopularClaimsDisponiveis(usuarioViewModel);
            await RemoverClaimsDisponiveisAtribuidas(usuarioViewModel);
            await PopularEmpresasDropdown(usuarioViewModel);
            await SelecionarPermissoesEmpresasDropdown(usuarioViewModel);

            return View(usuarioViewModel);
        }

        [ClaimsAuthorize("usuario", "n2")]
        [Route("usuario-adicionar-permissao")]
        [HttpPost]
        public async Task<IActionResult> AdicionarPermissao(Guid id, string type, string value)
        {
            await _usuarioApplication.AdicionarClaim(new ClaimViewModel
            {
                UserId = id.ToString(),
                Type = type,
                Value = value
            });

            if (!OperacaoValida())
            {
                TempData["Erro"] = string.Join("<br />", ObterNotificacoes());
            }

            return RedirectToAction("Permissoes", new { id });
        }

        [ClaimsAuthorize("usuario", "n3")]
        [Route("usuario-remover-permissoes")]
        [HttpPost]
        public async Task<IActionResult> RemoverPermissao(Guid id, string type, string value)
        {
            await _usuarioApplication.RemoverClaim(new ClaimViewModel
            {
                UserId = id.ToString(),
                Type = type,
                Value = value
            });

            if (!OperacaoValida())
            {
                TempData["Erro"] = string.Join("<br />", ObterNotificacoes());
            }

            return RedirectToAction("Permissoes", new { id });
        }

        [ClaimsAuthorize("usuario", "n2")]
        [Route("usuario-relacionar-empresa")]
        public async Task<IActionResult> RelacionarEmpresa(Guid usuarioId, int empresaId, bool selecionada)
        {
            if (!selecionada)
            {
                var empresa = await _empresaApplication.ObterEmpresa(empresaId);

                await _relUsuarioEmpresaApplication.Adicionar(new RelUsuarioEmpresaViewModel
                {
                    UsuarioId = usuarioId,
                    EmpresaId = empresa.Id,
                    EmpresaRazaoSocial = empresa.RazaoSocial,
                    EmpresaNomeFantasia = empresa.NomeFantasia
                });
            }
            else
            {
                var usuarioEmpresas = await _relUsuarioEmpresaApplication.ObterRelUsuarioEmpresaPorUsuario(usuarioId);

                var relacao = usuarioEmpresas.FirstOrDefault(e => e.UsuarioId == usuarioId && e.EmpresaId == empresaId);

                await _relUsuarioEmpresaApplication.Remover(relacao.Id);
            }

            var url = Url.Action("ObterUsuarioEmpresas", new { id = usuarioId });
            return Json(new { success = true, url });
        }

        public async Task<IActionResult> ObterUsuarioEmpresas(Guid id)
        {
            var usuarioViewModel = await _usuarioApplication.ObterUsuario(id);

            if (usuarioViewModel == null) return NotFound();

            await PopularEmpresasDropdown(usuarioViewModel);
            await SelecionarPermissoesEmpresasDropdown(usuarioViewModel);

            return PartialView("_RelacionarEmpresas", usuarioViewModel);
        }

        [ClaimsAuthorize("auditoria", "n1")]
        public async Task<IActionResult> Auditoria(Guid chave)
        {
            var usuarioViewModel = await _usuarioApplication.ObterUsuarioAuditoria(chave);

            if (usuarioViewModel == null) return NotFound();

            var auditoriasViewModel = await _auditoriaApplication.ObterAuditorias("Usuario", chave);

            auditoriasViewModel.AddRange(await _auditoriaApplication.ObterAuditorias("AspNetUserClaims", chave));

            foreach (var empresa in usuarioViewModel.Empresas)
            {
                auditoriasViewModel.AddRange(await _auditoriaApplication.ObterAuditorias("RelUsuarioEmpresa", empresa.Id));
            }

            TempData["AuditoriaRouteController"] = "Usuarios";
            TempData["AuditoriaRouteChave"] = chave.ToString();

            return PartialView("../Auditorias/_Auditoria", auditoriasViewModel.OrderBy(a => a.Data).ToList());
        }

        private async Task<UsuarioViewModel> PopularCargosDropdown(UsuarioViewModel usuarioViewModel)
        {
            var cargosViewModel = await _cargoApplication.ObterTodos();

            usuarioViewModel.CargosDropdown = cargosViewModel.OrderBy(c => c.Descricao).ToList();

            return usuarioViewModel;
        }

        private async Task<UsuarioViewModel> PopularUsuariosSapDropdown(UsuarioViewModel usuarioViewModel)
        {
            var usuariosSapViewModel = await _usuarioApplication.ObterUsuariosSap();

            usuarioViewModel.UsuariosSapDropdown = usuariosSapViewModel.OrderBy(u => u.Nome).ToList();

            return usuarioViewModel;
        }

        private async Task<UsuarioViewModel> PopularVendedoresSapDropdown(UsuarioViewModel usuarioViewModel)
        {
            var vendedoresSapViewModel = await _vendedorApplication.ObterTodos();

            usuarioViewModel.VendedoresSapDropdown = vendedoresSapViewModel.OrderBy(v => v.Nome).ToList();

            return usuarioViewModel;
        }

        private async Task<UsuarioViewModel> PopularEmpresasDropdown(UsuarioViewModel usuarioViewModel)
        {
            var empresasViewModel = await _empresaApplication.ObterTodos();

            if (empresasViewModel == null) return usuarioViewModel;

            usuarioViewModel.EmpresasDropdown = empresasViewModel.OrderBy(e => e.RazaoSocial).ToList();

            return usuarioViewModel;
        }

        private Task<UsuarioViewModel> RemoverClaimsDisponiveisAtribuidas(UsuarioViewModel usuarioViewModel)
        {
            foreach (var claim in usuarioViewModel.Claims)
            {
                usuarioViewModel.ClaimsDisponiveis.RemoveAll(c => c.Type == claim.Type);
            }

            return Task.FromResult(usuarioViewModel);
        }

        private Task<UsuarioViewModel> SelecionarPermissoesEmpresasDropdown(UsuarioViewModel usuarioViewModel)
        {
            if (usuarioViewModel.EmpresasDropdown == null || usuarioViewModel.EmpresasDropdown.Count == 0) return Task.FromResult(usuarioViewModel);

            if (usuarioViewModel.Empresas == null || usuarioViewModel.Empresas.Count == 0) return Task.FromResult(usuarioViewModel);

            foreach (var item in usuarioViewModel.Empresas)
            {
                usuarioViewModel.EmpresasDropdown.FirstOrDefault(e => e.Id == item.EmpresaId).Selecionada = true;
            }

            return Task.FromResult(usuarioViewModel);
        }

        private async Task<UsuarioViewModel> PopularClaimsDisponiveis(UsuarioViewModel usuarioViewModel)
        {
            var claimsViewModel = await _usuarioApplication.ObterClaimsDisponiveis();

            usuarioViewModel.ClaimsDisponiveis = claimsViewModel.OrderBy(c => c.Type).ThenBy(c => c.Value).ToList();

            return usuarioViewModel;
        }
    }
}