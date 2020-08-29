using AutoMapper;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.SapService.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Services
{
    public class ProdutoTipoApplication : BaseApplication, IProdutoTipoApplication
    {
        private readonly IProdutoTipoSapService _produtoTipoSapService;
        private readonly IMapper _mapper;

        public ProdutoTipoApplication(
            IProdutoTipoSapService produtoTipoSapService,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _produtoTipoSapService = produtoTipoSapService;
            _mapper = mapper;
        }

        public async Task<ProdutoTipoSapViewModel> ObterTipo(int id)
        {
            return _mapper.Map<ProdutoTipoSapViewModel>(await _produtoTipoSapService.ObterTipo(id));
        }

        public async Task<List<ProdutoTipoSapViewModel>> ObterTodos()
        {
            return _mapper.Map<List<ProdutoTipoSapViewModel>>(await _produtoTipoSapService.ObterTodos());
        }

        public void Dispose()
        {
            _produtoTipoSapService?.Dispose();
        }
    }
}
