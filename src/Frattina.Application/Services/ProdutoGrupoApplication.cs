using AutoMapper;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.SapService.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Services
{
    public class ProdutoGrupoApplication : BaseApplication, IProdutoGrupoApplication
    {
        private readonly IProdutoGrupoSapService _produtoGrupoSapService;
        private readonly IMapper _mapper;

        public ProdutoGrupoApplication(
            IProdutoGrupoSapService produtoGrupoSapService,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _produtoGrupoSapService = produtoGrupoSapService;
            _mapper = mapper;
        }

        public async Task<ProdutoGrupoSapViewModel> ObterGrupo(int id)
        {
            return _mapper.Map<ProdutoGrupoSapViewModel>(await _produtoGrupoSapService.ObterGrupo(id));
        }

        public async Task<List<ProdutoGrupoSapViewModel>> ObterTodos()
        {
            return _mapper.Map<List<ProdutoGrupoSapViewModel>>(await _produtoGrupoSapService.ObterTodos());
        }

        public void Dispose()
        {
            _produtoGrupoSapService?.Dispose();
        }
    }
}
