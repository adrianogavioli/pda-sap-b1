using AutoMapper;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.SapService.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Services
{
    public class ProdutoMarcaApplication : BaseApplication, IProdutoMarcaApplication
    {
        private readonly IProdutoMarcaSapService _ProdutoMarcaSapService;
        private readonly IMapper _mapper;

        public ProdutoMarcaApplication(
            IProdutoMarcaSapService ProdutoMarcaSapService,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _ProdutoMarcaSapService = ProdutoMarcaSapService;
            _mapper = mapper;
        }

        public async Task<ProdutoMarcaSapViewModel> ObterMarca(int id)
        {
            return _mapper.Map<ProdutoMarcaSapViewModel>(await _ProdutoMarcaSapService.ObterMarca(id));
        }

        public async Task<List<ProdutoMarcaSapViewModel>> ObterPorTipo(int tipoId)
        {
            return _mapper.Map<List<ProdutoMarcaSapViewModel>>(await _ProdutoMarcaSapService.ObterPorTipo(tipoId));
        }

        public async Task<List<ProdutoMarcaSapViewModel>> ObterTodos()
        {
            return _mapper.Map<List<ProdutoMarcaSapViewModel>>(await _ProdutoMarcaSapService.ObterTodos());
        }

        public void Dispose()
        {
            _ProdutoMarcaSapService?.Dispose();
        }
    }
}
