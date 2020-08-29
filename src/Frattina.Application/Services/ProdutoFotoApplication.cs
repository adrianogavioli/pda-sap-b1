using AutoMapper;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.SapService.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.Application.Services
{
    public class ProdutoFotoApplication : BaseApplication, IProdutoFotoApplication
    {
        private readonly IProdutoFotoSapService _ProdutoFotoSapService;
        private readonly IMapper _mapper;

        public ProdutoFotoApplication(
            IProdutoFotoSapService ProdutoFotoSapService,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _ProdutoFotoSapService = ProdutoFotoSapService;
            _mapper = mapper;
        }

        public async Task<ProdutoFotoSapViewModel> ObterFoto(int id)
        {
            return _mapper.Map<ProdutoFotoSapViewModel>(await _ProdutoFotoSapService.ObterFoto(id));
        }

        public async Task<List<ProdutoFotoSapViewModel>> ObterPorProduto(string produtoId)
        {
            var produtoFotosViewModel = _mapper.Map<List<ProdutoFotoSapViewModel>>(await _ProdutoFotoSapService.ObterPorItemCode(produtoId));

            return produtoFotosViewModel.OrderBy(f => f.Grupo).ThenByDescending(f => f.Principal).ToList();
        }

        public void Dispose()
        {
            _ProdutoFotoSapService?.Dispose();
        }
    }
}
