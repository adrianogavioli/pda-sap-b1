using AutoMapper;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Models.Enums;
using Frattina.SapService.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.Application.Services
{
    public class ProdutoApplication : BaseApplication, IProdutoApplication
    {
        private readonly IProdutoSapService _produtoSapService;
        private readonly IProdutoEstoqueApplication _produtoEstoqueApplication;
        private readonly IProdutoPrecoApplication _produtoPrecoApplication;
        private readonly IProdutoFotoApplication _produtoFotoApplication;
        private readonly IProdutoFichaTecnicaApplication _produtoFichaTecnicaApplication;
        private readonly IAtendimentoProdutoRepository _atendimentoProdutoRepository;
        private readonly IMapper _mapper;

        public ProdutoApplication(
            IProdutoSapService produtoSapService,
            IProdutoEstoqueApplication produtoEstoqueApplication,
            IProdutoPrecoApplication produtoPrecoApplication,
            IProdutoFotoApplication produtoFotoApplication,
            IProdutoFichaTecnicaApplication produtoFichaTecnicaApplication,
            IAtendimentoProdutoRepository atendimentoProdutoRepository,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _produtoSapService = produtoSapService;
            _produtoEstoqueApplication = produtoEstoqueApplication;
            _produtoPrecoApplication = produtoPrecoApplication;
            _produtoFotoApplication = produtoFotoApplication;
            _produtoFichaTecnicaApplication = produtoFichaTecnicaApplication;
            _atendimentoProdutoRepository = atendimentoProdutoRepository;
            _mapper = mapper;
        }

        public async Task<ProdutoSapViewModel> ObterProduto(string id)
        {
            var produtoSapViewModel = _mapper.Map<ProdutoSapViewModel>(await _produtoSapService.ObterProduto(id));

            if (produtoSapViewModel == null) return null;

            produtoSapViewModel.Estoques = await _produtoEstoqueApplication.ObterPorProduto(id);

            produtoSapViewModel.Precos = await _produtoPrecoApplication.ObterPorProduto(id);

            produtoSapViewModel.Fotos = await _produtoFotoApplication.ObterPorProduto(id);

            produtoSapViewModel.FichaTecnica = await _produtoFichaTecnicaApplication.ObterPorProduto(id);

            return produtoSapViewModel;
        }

        public async Task<List<ProdutoSapViewModel>> ObterPorCodigoMaster(string codigoMaster)
        {
            return _mapper.Map<List<ProdutoSapViewModel>>(await _produtoSapService.ObterPorMasterCode(codigoMaster));
        }

        public async Task<List<ProdutoSapViewModel>> ObterPorTipo(int tipoId)
        {
            return _mapper.Map<List<ProdutoSapViewModel>>(await _produtoSapService.ObterPorTipoCode(tipoId));
        }

        public async Task<List<ProdutoSapViewModel>> ObterPorMarca(int marcaId)
        {
            return _mapper.Map<List<ProdutoSapViewModel>>(await _produtoSapService.ObterPorMarcaCode(marcaId));
        }

        public async Task<List<ProdutoSapViewModel>> ObterPorModelo(int modeloId)
        {
            return _mapper.Map<List<ProdutoSapViewModel>>(await _produtoSapService.ObterPorModeloCode(modeloId));
        }

        public async Task<List<ProdutoSapViewModel>> ObterPorReferencia(string referencia)
        {
            return _mapper.Map<List<ProdutoSapViewModel>>(await _produtoSapService.ObterPorSupplierCatalog(referencia));
        }

        public async Task<List<ProdutoSapViewModel>> ObterPorTipoMarcaModelo(int? tipoId, int? marcaId, int? modeloId)
        {
            return _mapper.Map<List<ProdutoSapViewModel>>(await _produtoSapService.ObterPorTipoMarcaModeloCodes(tipoId, marcaId, modeloId));
        }

        public async Task<ProdutoVisaoViewModel> ObterVisao(string id)
        {
            var atendimentoProdutos = await _atendimentoProdutoRepository.ObterAtendimentoProdutosPorProduto(id);

            var ultimoAtendimento = atendimentoProdutos.Select(p => p.Atendimento)
                    .OrderByDescending(a => a.Data).FirstOrDefault();

            return new ProdutoVisaoViewModel
            {
                ProdutoId = id,
                QuantidadeAtendimentos = atendimentoProdutos.Count(),
                QuantidadeVendas = atendimentoProdutos.Count(p => p.Atendimento.Etapa == AtendimentoEtapa.Vendido && !p.RemovidoVenda),
                QuantidadeNaoGostou = atendimentoProdutos.Count(p => p.NivelInteresse == 1),
                QuantidadeGostou = atendimentoProdutos.Count(p => p.NivelInteresse == 2),
                QuantidadeAmou = atendimentoProdutos.Count(p => p.NivelInteresse == 3),
                DataUltimoAtendimento = ultimoAtendimento?.Data,
                VendedorUltimoAtendimento = ultimoAtendimento?.VendedorNome
            };
        }

        public void Dispose()
        {
            _produtoSapService?.Dispose();
            _produtoEstoqueApplication?.Dispose();
            _produtoPrecoApplication?.Dispose();
            _produtoFotoApplication?.Dispose();
            _produtoFichaTecnicaApplication?.Dispose();
        }
    }
}
