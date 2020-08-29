using AutoMapper;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.SapService.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Services
{
    public class ProdutoModeloApplication : BaseApplication, IProdutoModeloApplication
    {
        private readonly IProdutoModeloSapService _ProdutoModeloSapService;
        private readonly IMapper _mapper;

        public ProdutoModeloApplication(
            IProdutoModeloSapService ProdutoModeloSapService,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _ProdutoModeloSapService = ProdutoModeloSapService;
            _mapper = mapper;
        }

        public async Task<ProdutoModeloSapViewModel> ObterModelo(int id)
        {
            return _mapper.Map<ProdutoModeloSapViewModel>(await _ProdutoModeloSapService.ObterModelo(id));
        }

        public async Task<List<ProdutoModeloSapViewModel>> ObterPorTipo(int tipoId)
        {
            return _mapper.Map<List<ProdutoModeloSapViewModel>>(await _ProdutoModeloSapService.ObterPorTipo(tipoId));
        }

        public async Task<List<ProdutoModeloSapViewModel>> ObterPorMarca(int marcaId)
        {
            return _mapper.Map<List<ProdutoModeloSapViewModel>>(await _ProdutoModeloSapService.ObterPorMarca(marcaId));
        }

        public async Task<List<ProdutoModeloSapViewModel>> ObterPorTipoMarca(int tipoId, int marcaId)
        {
            return _mapper.Map<List<ProdutoModeloSapViewModel>>(await _ProdutoModeloSapService.ObterPorTipoMarca(tipoId, marcaId));
        }

        public async Task<List<ProdutoModeloSapViewModel>> ObterTodos()
        {
            return _mapper.Map<List<ProdutoModeloSapViewModel>>(await _ProdutoModeloSapService.ObterTodos());
        }

        public void Dispose()
        {
            _ProdutoModeloSapService?.Dispose();
        }
    }
}
