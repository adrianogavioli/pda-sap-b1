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
    public class ProdutoEstoqueApplication : BaseApplication, IProdutoEstoqueApplication
    {
        private readonly IProdutoEstoqueSapService _produtoEstoqueSapService;
        private readonly IMapper _mapper;

        public ProdutoEstoqueApplication(
            IProdutoEstoqueSapService produtoEstoqueSapService,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _produtoEstoqueSapService = produtoEstoqueSapService;
            _mapper = mapper;
        }

        public async Task<List<ProdutoEstoqueSapViewModel>> ObterPorProduto(string produtoId)
        {
            var produtoEstoquesViewModel =  _mapper.Map<List<ProdutoEstoqueSapViewModel>>(await _produtoEstoqueSapService.ObterPorItemCode(produtoId));

            return produtoEstoquesViewModel.OrderBy(e => e.Estoque).ToList();
        }

        public void Dispose()
        {
            _produtoEstoqueSapService?.Dispose();
        }
    }
}
