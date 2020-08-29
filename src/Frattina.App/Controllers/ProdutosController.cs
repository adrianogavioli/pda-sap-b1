using Frattina.App.Extensions;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.App.Controllers
{
    [Authorize]
    public class ProdutosController : BaseController
    {
        private readonly IProdutoApplication _produtoApplication;
        private readonly IProdutoTipoApplication _produtoTipoApplication;
        private readonly IProdutoMarcaApplication _produtoMarcaApplication;
        private readonly IProdutoModeloApplication _produtoModeloApplication;
        private readonly IUsuarioProdutoConsultaApplication _usuarioProdutoConsultaApplication;

        public ProdutosController(IProdutoApplication produtoApplication,
            IProdutoTipoApplication produtoTipoApplication,
            IProdutoMarcaApplication produtoMarcaApplication,
            IProdutoModeloApplication produtoModeloApplication,
            IUsuarioProdutoConsultaApplication usuarioProdutoConsultaApplication,
            INotificador notificador) : base(notificador)
        {
            _produtoApplication = produtoApplication;
            _produtoTipoApplication = produtoTipoApplication;
            _produtoMarcaApplication = produtoMarcaApplication;
            _produtoModeloApplication = produtoModeloApplication;
            _usuarioProdutoConsultaApplication = usuarioProdutoConsultaApplication;
        }

        [ClaimsAuthorize("produto", "n1")]
        [Route("produto-consulta-catalogo")]
        public async Task<ActionResult> ConsultaCatalogo(string idFilter, string codigoMasterFilter, string referenciaFilter, string tipoFilter, string marcaFilter, string modeloFilter)
        {
            var searchFilters = await GerarFiltrosPesquisas(idFilter, codigoMasterFilter, referenciaFilter, tipoFilter, marcaFilter, modeloFilter);

            SearchFiltersFactory(searchFilters);

            var idSearchFilter = searchFilters[0];
            var codigoMasterSearchFilter = searchFilters[1];
            var referenciaSearchFilter = searchFilters[2];
            var tipoSearchFilter = searchFilters[3];
            var marcaSearchFilter = searchFilters[4];
            var modeloSearchFilter = searchFilters[5];

            var tipoIdFilter = tipoSearchFilter.ValueFilter == null ? 0 : Convert.ToInt32(tipoSearchFilter.ValueFilter);
            var marcaIdFilter = marcaSearchFilter.ValueFilter == null ? 0 : Convert.ToInt32(marcaSearchFilter.ValueFilter);

            var produtosViewModel = new List<ProdutoSapViewModel>();

            ViewBag.TiposDropdown = await PopularTiposDropdown();
            ViewBag.MarcasDropdown = await PopularMarcasDropdown(tipoIdFilter);
            ViewBag.ModelosDropdown = await PopularModelosDropdown(tipoIdFilter, marcaIdFilter);

            if (idSearchFilter?.ValueFilter != null)
            {
                var produtoViewModel = await _produtoApplication.ObterProduto((string)idSearchFilter.ValueFilter);

                if (produtoViewModel != null)
                    produtosViewModel.Add(produtoViewModel);
            }
            else if (codigoMasterSearchFilter?.ValueFilter != null)
            {
                produtosViewModel.AddRange(await _produtoApplication.ObterPorCodigoMaster((string)codigoMasterSearchFilter.ValueFilter));
            }
            else if (referenciaSearchFilter?.ValueFilter != null)
            {
                produtosViewModel.AddRange(await _produtoApplication.ObterPorReferencia((string)referenciaSearchFilter.ValueFilter));
            }
            else if (tipoSearchFilter?.ValueFilter != null
                    || marcaSearchFilter?.ValueFilter != null
                    || modeloSearchFilter?.ValueFilter != null)
            {
                var tipoCode = tipoSearchFilter.ValueFilter == null ? null : (int?)Convert.ToInt32(tipoSearchFilter.ValueFilter);
                var marcaCode = marcaSearchFilter.ValueFilter == null ? null : (int?)Convert.ToInt32(marcaSearchFilter.ValueFilter);
                var modeloCode = modeloSearchFilter.ValueFilter == null ? null : (int?)Convert.ToInt32(modeloSearchFilter.ValueFilter);

                produtosViewModel.AddRange(await _produtoApplication.ObterPorTipoMarcaModelo(tipoCode, marcaCode, modeloCode));
            }

            return View(produtosViewModel);
        }

        [ClaimsAuthorize("produto", "n1")]
        [Route("produto-consulta-rapida")]
        public async Task<ActionResult> ConsultaRapida(string id, string acao)
        {
            var produtoConsultaRapidaViewModel = new ProdutoConsultaRapidaViewModel
            {
                UsuarioProdutosConsultas = await _usuarioProdutoConsultaApplication.ObterUltimasVinteConsultasPorUsuarioAuth()
            };

            if (string.IsNullOrEmpty(id)) return View(produtoConsultaRapidaViewModel);

            var produtoViewModel = await _produtoApplication.ObterProduto(id);

            if (produtoViewModel == null)
            {
                TempData["Erro"] = "Nenhum produto foi encontrado.";

                return RedirectToAction("ConsultaRapida");
            }

            produtoConsultaRapidaViewModel.ProdutoConsulta = produtoViewModel;

            if (acao == "registrar")
            {
                await _usuarioProdutoConsultaApplication.Adicionar(new UsuarioProdutoConsultaViewModel
                {
                    ProdutoId = produtoViewModel.Id,
                    Imagem = produtoViewModel.Imagem
                });

                produtoConsultaRapidaViewModel.UsuarioProdutosConsultas = await _usuarioProdutoConsultaApplication.ObterUltimasVinteConsultasPorUsuarioAuth();
            }

            return View(produtoConsultaRapidaViewModel);
        }

        [ClaimsAuthorize("produto", "n1")]
        [Route("produto/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            var produtoViewModel =await _produtoApplication.ObterProduto(id);

            if (produtoViewModel == null) return NotFound();

            return View(produtoViewModel);
        }

        [ClaimsAuthorize("produto", "n1")]
        public async Task<ActionResult> Visao(string id)
        {
            return PartialView("_Visao", await _produtoApplication.ObterVisao(id));
        }

        private async Task<List<ProdutoTipoSapViewModel>> PopularTiposDropdown()
        {
            var produtoTiposViewModel = await _produtoTipoApplication.ObterTodos();

            return produtoTiposViewModel.OrderBy(t => t.Nome).ToList();
        }

        private async Task<List<ProdutoMarcaSapViewModel>> PopularMarcasDropdown(int tipoId)
        {
            var marcasViewModel = new List<ProdutoMarcaSapViewModel>();

            if (tipoId > 0)
                marcasViewModel = await _produtoMarcaApplication.ObterPorTipo(tipoId);

            return marcasViewModel.OrderBy(m => m.Nome).ToList();
        }

        private async Task<List<ProdutoModeloSapViewModel>> PopularModelosDropdown(int tipoId, int marcaId)
        {
            var modelosViewModel = new List<ProdutoModeloSapViewModel>();

            if (tipoId > 0 && marcaId > 0)
                modelosViewModel = await _produtoModeloApplication.ObterPorTipoMarca(tipoId, marcaId);

            return modelosViewModel.OrderBy(m => m.Nome).ToList();
        }

        [Route("produto-popular-marcas-portipo")]
        public async Task<IActionResult> PopularMarcasPorTipo(int tipoId)
        {
            return Json(JsonConvert.SerializeObject(await PopularMarcasDropdown(tipoId)));
        }

        [Route("produto-popular-modelos-portipomarca")]
        public async Task<IActionResult> PopularModelosPorTipoMarca(int tipoId, int marcaId)
        {
            return Json(JsonConvert.SerializeObject(await PopularModelosDropdown(tipoId, marcaId)));
        }

        public async Task<ActionResult> LimparFiltrosPesquisa(string idFilter, string codigoMasterFilter, string referenciaFilter, string tipoFilter, string marcaFilter, string modeloFilter)
        {
            CleanSearchFilters(await GerarFiltrosPesquisas(idFilter, codigoMasterFilter, referenciaFilter, tipoFilter, marcaFilter, modeloFilter));

            return RedirectToAction("ConsultaCatalogo");
        }

        private Task<List<SearchFilter>> GerarFiltrosPesquisas(string idFilter, string codigoMasterFilter, string referenciaFilter, string tipoFilter, string marcaFilter, string modeloFilter)
        {
            return Task.FromResult(new List<SearchFilter>
            {
                new SearchFilter { KeyFilter = "ProdutoIdFilter", ValueFilter = idFilter },
                new SearchFilter { KeyFilter = "ProdutoCodigoMasterFilter", ValueFilter = codigoMasterFilter },
                new SearchFilter { KeyFilter = "ProdutoReferenciaFilter", ValueFilter = referenciaFilter },
                new SearchFilter { KeyFilter = "ProdutoTipoFilter", ValueFilter = tipoFilter },
                new SearchFilter { KeyFilter = "ProdutoMarcaFilter", ValueFilter = marcaFilter },
                new SearchFilter { KeyFilter = "ProdutoModeloFilter", ValueFilter = modeloFilter }
            });
        }
    }
}