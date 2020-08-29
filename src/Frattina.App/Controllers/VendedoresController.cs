using Frattina.App.Extensions;
using Frattina.Application.Interfaces;
using Frattina.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Frattina.App.Controllers
{
    [Authorize]
    public class VendedoresController : BaseController
    {
        private readonly IVendedorApplication _vendedorApplication;

        public VendedoresController(IVendedorApplication vendedorApplication,
            INotificador notificador) : base(notificador)
        {
            _vendedorApplication = vendedorApplication;
        }

        [ClaimsAuthorize("vendedor", "n1")]
        [Route("vendedores")]
        public async Task<IActionResult> Index()
        {
            return View(await _vendedorApplication.ObterTodos());
        }

        [ClaimsAuthorize("vendedor", "n1")]
        [Route("vendedor")]
        public async Task<IActionResult> Details(int id)
        {
            var vendedorViewModel = await _vendedorApplication.ObterVendedor(id);

            if (vendedorViewModel == null) return NotFound();

            return View(vendedorViewModel);
        }

        [ClaimsAuthorize("vendedor", "n1")]
        public async Task<IActionResult> Visao(int id)
        {
            return PartialView("_Visao", await _vendedorApplication.ObterVisao(id));
        }
    }
}