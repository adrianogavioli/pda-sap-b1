using AutoMapper;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.SapService.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Services
{
    public class EstoqueApplication : BaseApplication, IEstoqueApplication
    {
        private readonly IEstoqueSapService _estoqueSapService;
        private readonly IMapper _mapper;

        public EstoqueApplication(
            IEstoqueSapService estoqueSapService,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _estoqueSapService = estoqueSapService;
            _mapper = mapper;
        }

        public async Task<List<EstoqueSapViewModel>> ObterPorEmpresa(int EmpresaId)
        {
            return _mapper.Map<List<EstoqueSapViewModel>>(await _estoqueSapService.ObterPorEmpresa(EmpresaId));
        }

        public async Task<List<EstoqueSapViewModel>> ObterTodos()
        {
            return _mapper.Map<List<EstoqueSapViewModel>>(await _estoqueSapService.ObterTodos());
        }

        public void Dispose()
        {
            _estoqueSapService?.Dispose();
        }
    }
}
