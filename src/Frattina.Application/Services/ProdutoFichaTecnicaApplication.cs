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
    public class ProdutoFichaTecnicaApplication : BaseApplication, IProdutoFichaTecnicaApplication
    {
        private readonly IProdutoFichaTecnicaSapService _produtoFichaTecnicaSapService;
        private readonly IMapper _mapper;

        public ProdutoFichaTecnicaApplication(
            IProdutoFichaTecnicaSapService produtoFichaTecnicaSapService,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _produtoFichaTecnicaSapService = produtoFichaTecnicaSapService;
            _mapper = mapper;
        }

        public async Task<List<ProdutoFichaTecnicaSapViewModel>> ObterPorProduto(string produtoId)
        {
            var produtoFichaTecnicaViewModel = _mapper.Map<List<ProdutoFichaTecnicaSapViewModel>>(await _produtoFichaTecnicaSapService.ObterPorItemCode(produtoId));

            return produtoFichaTecnicaViewModel.OrderBy(f => f.Caracteristica).ThenBy(f => f.Detalhe).ToList();
        }

        public void Dispose()
        {
            _produtoFichaTecnicaSapService?.Dispose();
        }
    }
}
