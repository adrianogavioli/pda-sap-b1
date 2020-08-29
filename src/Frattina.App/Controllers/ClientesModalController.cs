using Frattina.App.Extensions;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.App.Controllers
{
    [Authorize]
    public class ClientesModalController : BaseController
    {
        private readonly IClienteApplication _clienteApplication;
        
        public ClientesModalController(IClienteApplication clienteApplication,
            INotificador notificador) : base(notificador)
        {
            _clienteApplication = clienteApplication;
        }

        [ClaimsAuthorize("cliente", "n1")]
        public async Task<IActionResult> DetalharClienteModal(string id)
        {
            var clienteViewModel = await _clienteApplication.ObterCliente(id);

            if (clienteViewModel == null) return NotFound();

            return PartialView("_DetalharClienteModal", clienteViewModel);
        }

        [ClaimsAuthorize("cliente", "n2")]
        public async Task<IActionResult> EditarClienteModal(string id)
        {
            var clienteViewModel = await _clienteApplication.ObterCliente(id);

            return PartialView("_EditarClienteModal", clienteViewModel);
        }

        [ClaimsAuthorize("cliente", "n2")]
        [HttpPost]
        public async Task<ActionResult> EditarClienteModal(string id, ClienteSapViewModel clienteViewModel)
        {
            if (id != clienteViewModel.Id) return NotFound();

            var clienteDb = await _clienteApplication.ObterCliente(id);

            clienteViewModel.Enderecos = clienteDb.Enderecos;

            if (!ModelState.IsValid) return PartialView("_EditarClienteModal", clienteViewModel);

            await _clienteApplication.Atualizar(clienteViewModel);

            if (!OperacaoValida()) return PartialView("_EditarClienteModal", clienteViewModel);

            TempData["Sucesso"] = "O cliente foi atualizado.";

            var url = HttpContext.Request.GetTypedHeaders().Referer.AbsoluteUri;
            return Json(new { success = true, url });
        }

        [ClaimsAuthorize("cliente", "n1")]
        public async Task<IActionResult> DetalharEnderecoModal(string clienteId, int numeroLinha)
        {
            var clienteViewModel = await _clienteApplication.ObterCliente(clienteId);

            if (clienteViewModel == null) return NotFound();

            var clienteEnderecoViewModel = clienteViewModel.Enderecos.FirstOrDefault(e => e.NumeroLinha == numeroLinha);

            if (clienteEnderecoViewModel == null) return NotFound();

            return PartialView("_DetalharEnderecoModal", clienteEnderecoViewModel);
        }

        [ClaimsAuthorize("cliente", "n2")]
        public IActionResult AdicionarEnderecoModal(string clienteId)
        {
            var clienteEnderecoViewModel = new ClienteEnderecoSapViewModel
            {
                ClienteId = clienteId,
                EnderecoExterior = false
            };

            return PartialView("_AdicionarEnderecoModal", clienteEnderecoViewModel);
        }

        [ClaimsAuthorize("cliente", "n2")]
        [HttpPost]
        public async Task<IActionResult> AdicionarEnderecoModal(ClienteEnderecoSapViewModel clienteEnderecoViewModel)
        {
            ModelState.Remove("AddressName");

            if (!ModelState.IsValid) return PartialView("_AdicionarEnderecoModal", clienteEnderecoViewModel);

            await _clienteApplication.AdicionarEndereco(clienteEnderecoViewModel);

            if (!OperacaoValida()) return PartialView("_AdicionarEnderecoModal", clienteEnderecoViewModel);

            return await EditarClienteModal(clienteEnderecoViewModel.ClienteId);
        }

        [ClaimsAuthorize("cliente", "n2")]
        public async Task<IActionResult> EditarEnderecoModal(string clienteId, int numeroLinha)
        {
            var clienteViewModel = await _clienteApplication.ObterCliente(clienteId);

            if (clienteViewModel == null) return NotFound();

            var clienteEnderecoViewModel = clienteViewModel.Enderecos.FirstOrDefault(e => e.NumeroLinha == numeroLinha);

            if (clienteEnderecoViewModel == null) return NotFound();

            clienteEnderecoViewModel.EnderecoExterior = clienteEnderecoViewModel.Cep == "99999999";

            return PartialView("_EditarEnderecoModal", clienteEnderecoViewModel);
        }

        [ClaimsAuthorize("cliente", "n2")]
        [HttpPost]
        public async Task<IActionResult> EditarEnderecoModal(string clienteId, int numeroLinha, ClienteEnderecoSapViewModel clienteEnderecoViewModel)
        {
            if (clienteId != clienteEnderecoViewModel.ClienteId || numeroLinha != clienteEnderecoViewModel.NumeroLinha) return NotFound();

            if (!ModelState.IsValid) return PartialView("_EditarEnderecoModal", clienteEnderecoViewModel);

            await _clienteApplication.AtualizarEndereco(clienteEnderecoViewModel);

            if (!OperacaoValida()) return PartialView("_EditarEnderecoModal", clienteEnderecoViewModel);

            return await EditarClienteModal(clienteId);
        }
    }
}