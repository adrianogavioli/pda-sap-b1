using AutoMapper;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.SapService.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Services
{
    public class ClienteGrupoApplication : BaseApplication, IClienteGrupoApplication
    {
        private readonly IClienteGrupoSapService _clienteGrupoSapService;
        private readonly IMapper _mapper;

        public ClienteGrupoApplication(
            IClienteGrupoSapService clienteGrupoSapService,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _clienteGrupoSapService = clienteGrupoSapService;
            _mapper = mapper;
        }

        public async Task<ClienteGrupoSapViewModel> ObterGrupo(int id)
        {
            return _mapper.Map<ClienteGrupoSapViewModel>(await _clienteGrupoSapService.ObterGrupo(id));
        }

        public async Task<List<ClienteGrupoSapViewModel>> ObterTodos()
        {
            return _mapper.Map<List<ClienteGrupoSapViewModel>>(await _clienteGrupoSapService.ObterTodos());
        }

        public void Dispose()
        {
            _clienteGrupoSapService?.Dispose();
        }
    }
}
