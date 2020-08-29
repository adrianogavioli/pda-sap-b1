using Frattina.App.Extensions;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Frattina.App.Controllers
{
    [Authorize]
    public class CargosController : BaseController
    {
        private readonly ICargoApplication _cargoApplication;

        public CargosController(
            ICargoApplication cargoApplication,
            INotificador notificador) : base(notificador)
        {
            _cargoApplication = cargoApplication;
        }

        [ClaimsAuthorize("cargo", "n1")]
        [Route("cargos")]
        public async Task<IActionResult> Index()
        {
            return View(await _cargoApplication.ObterTodos());
        }

        [ClaimsAuthorize("cargo", "n1")]
        [Route("cargo/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var cargoViewModel = await _cargoApplication.ObterCargoUsuarios(id);

            if (cargoViewModel == null) return NotFound();

            return View(cargoViewModel);
        }

        [ClaimsAuthorize("cargo", "n2")]
        [Route("adicionar-cargo")]
        public IActionResult Create()
        {
            return View();
        }

        [ClaimsAuthorize("cargo", "n2")]
        [Route("adicionar-cargo")]
        [HttpPost]
        public async Task<IActionResult> Create(CargoViewModel cargoViewModel)
        {
            if (!ModelState.IsValid) return View(cargoViewModel);

            await _cargoApplication.Adicionar(cargoViewModel);

            if (!OperacaoValida()) return View(cargoViewModel);

            TempData["Sucesso"] = "O novo cargo foi adicionado.";

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("cargo", "n2")]
        [Route("editar-cargo/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var cargoViewModel = await _cargoApplication.ObterCargoUsuarios(id);

            if (cargoViewModel == null) return NotFound();

            return View(cargoViewModel);
        }

        [ClaimsAuthorize("cargo", "n2")]
        [Route("editar-cargo/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, CargoViewModel cargoViewModel)
        {
            if (id != cargoViewModel.Id) return NotFound();

            var cargoViewModelReturn = await _cargoApplication.ObterCargoUsuarios(id);

            if (cargoViewModelReturn == null) return NotFound();

            if (!ModelState.IsValid) return View(cargoViewModelReturn);

            await _cargoApplication.Atualizar(cargoViewModel);

            if (!OperacaoValida()) return View(cargoViewModelReturn);

            TempData["Sucesso"] = "O cargo foi atualizado.";

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("cargo", "n3")]
        [Route("remover-cargo/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cargoViewModel = await _cargoApplication.ObterCargo(id);

            if (cargoViewModel == null) return NotFound();

            return View(cargoViewModel);
        }

        [ClaimsAuthorize("cargo", "n3")]
        [Route("remover-cargo/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cargoViewModel = await _cargoApplication.ObterCargo(id);

            if (cargoViewModel == null) return NotFound();

            await _cargoApplication.Remover(id);

            if (!OperacaoValida()) return View(cargoViewModel);

            TempData["Sucesso"] = "O cargo foi removido";

            return RedirectToAction("Index");
        }
    }
}
