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
    public class ProdutoPrecoApplication : BaseApplication, IProdutoPrecoApplication
    {
        private readonly IProdutoPrecoSapService _ProdutoPrecoSapService;
        private readonly IMapper _mapper;

        public ProdutoPrecoApplication(
            IProdutoPrecoSapService produtoPrecoSapService,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _ProdutoPrecoSapService = produtoPrecoSapService;
            _mapper = mapper;
        }

        public async Task<List<ProdutoPrecoSapViewModel>> ObterPorProduto(string produtoId)
        {
            var produtoPrecosViewModel = _mapper.Map<List<ProdutoPrecoSapViewModel>>(await _ProdutoPrecoSapService.ObterPorItemCode(produtoId));

            return produtoPrecosViewModel.OrderBy(p => p.Parcela).ToList();
        }

        public void Dispose()
        {
            _ProdutoPrecoSapService?.Dispose();
        }
    }
}
