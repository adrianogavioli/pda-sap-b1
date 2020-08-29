using Frattina.App.Extensions;
using Frattina.Application.Interfaces;
using Frattina.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Frattina.App.Controllers
{
    [Authorize]
    public class ProdutosModalController : BaseController
    {
        private readonly IProdutoApplication _produtoApplication;

        public ProdutosModalController(
            IProdutoApplication produtoApplication,
            INotificador notificador) : base(notificador)
        {
            _produtoApplication = produtoApplication;
        }

        [ClaimsAuthorize("produto", "n1")]
        public async Task<IActionResult> DetalharProdutoModal(string id)
        {
            var produtoViewModel = await _produtoApplication.ObterProduto(id);

            if (produtoViewModel == null) return NotFound();

            return PartialView("_DetalharProdutoModal", produtoViewModel);
        }
    }
}
