using AutoMapper;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.SapService.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Services
{
    public class EmpresaApplication : BaseApplication, IEmpresaApplication
    {
        private readonly IEmpresaSapService _empresaSapService;
        private readonly IMapper _mapper;

        public EmpresaApplication(
            IEmpresaSapService empresaSapService,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _empresaSapService = empresaSapService;
            _mapper = mapper;
        }

        public async Task<EmpresaSapViewModel> ObterEmpresa(int id)
        {
            return _mapper.Map<EmpresaSapViewModel>(await _empresaSapService.ObterEmpresa(id));
        }

        public async Task<List<EmpresaSapViewModel>> ObterTodos()
        {
            return _mapper.Map<List<EmpresaSapViewModel>>(await _empresaSapService.ObterTodos());
        }

        public void Dispose()
        {
            _empresaSapService?.Dispose();
        }
    }
}
