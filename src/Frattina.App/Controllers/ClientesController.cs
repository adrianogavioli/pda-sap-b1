using Frattina.App.Extensions;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.App.Controllers
{
    [Authorize]
    public class ClientesController : BaseController
    {
        private readonly IClienteApplication _clienteApplication;
        
        public ClientesController(IClienteApplication clienteApplication,
            INotificador notificador) : base(notificador)
        {
            _clienteApplication = clienteApplication;
        }

        [ClaimsAuthorize("cliente", "n1")]
        [Route("clientes")]
        public async Task<IActionResult> Index(string cpfFilter, string cnpjFilter, string nomeFilter)
        {
            var searchFilters = await GerarFiltrosPesquisas(cpfFilter, cnpjFilter, nomeFilter);

            SearchFiltersFactory(searchFilters);

            var clientesViewModel = new List<ClienteSapViewModel>();

            var cpfSearchFilter = searchFilters[0];
            var cnpjSearchFilter = searchFilters[1];
            var nomeSearchFilter = searchFilters[2];

            if (cpfSearchFilter?.ValueFilter != null)
            {
                var clienteViewModel = await _clienteApplication.ObterPorCPF((string)cpfSearchFilter.ValueFilter);

                if (clienteViewModel != null)
                    clientesViewModel.Add(clienteViewModel);
            }
            else if (cnpjSearchFilter?.ValueFilter != null)
            {
                var clienteViewModel = await _clienteApplication.ObterPorCNPJ((string)cnpjSearchFilter.ValueFilter);

                if (clienteViewModel != null)
                    clientesViewModel.Add(clienteViewModel);
            }
            else if (nomeSearchFilter?.ValueFilter != null)
                clientesViewModel.AddRange(await _clienteApplication.ObterPorPartNome((string)nomeSearchFilter.ValueFilter));
           
            return View(clientesViewModel);
        }

        [ClaimsAuthorize("cliente", "n1")]
        [Route("cliente/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            var clienteViewModel = await _clienteApplication.ObterCliente(id);

            if (clienteViewModel == null) return NotFound();

            return View(clienteViewModel);
        }

        [ClaimsAuthorize("cliente", "n2")]
        [Route("adicionar-cliente")]
        public IActionResult Create()
        {
            return View(new ClienteSapViewModel
            {
                Contribuinte = false
            });
        }

        [ClaimsAuthorize("cliente", "n2")]
        [Route("adicionar-cliente")]
        [HttpPost]
        public async Task<IActionResult> Create(ClienteSapViewModel clienteViewModel)
        {
            if (!ModelState.IsValid) return View(clienteViewModel);

            var clienteAdd = await _clienteApplication.Adicionar(clienteViewModel);

            if (!OperacaoValida()) return View(clienteViewModel);

            TempData["Sucesso"] = "O cliente foi adicionado.";

            return RedirectToAction("Edit", new { id = clienteAdd.Id });
        }

        [ClaimsAuthorize("cliente", "n2")]
        [Route("editar-cliente/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var clienteViewModel = await _clienteApplication.ObterCliente(id);

            if (clienteViewModel == null) return NotFound();

            return View(clienteViewModel);
        }

        [ClaimsAuthorize("cliente", "n2")]
        [Route("editar-cliente/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, ClienteSapViewModel clienteSapViewModel)
        {
            if (id != clienteSapViewModel.Id) return NotFound();

            var clienteDb = await _clienteApplication.ObterCliente(id);

            if (clienteDb == null) return NotFound();

            clienteSapViewModel.Enderecos = clienteDb.Enderecos;
            clienteSapViewModel.Contatos = clienteDb.Contatos;

            if (!ModelState.IsValid) return View(clienteSapViewModel);

            await _clienteApplication.Atualizar(clienteSapViewModel);

            if (!OperacaoValida()) return View(clienteSapViewModel);

            TempData["Sucesso"] = "O cliente foi atualizado.";

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("cliente", "n3")]
        [Route("remover-cliente/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var clienteViewModel = await _clienteApplication.ObterCliente(id);

            if (clienteViewModel == null) return NotFound();

            return View(clienteViewModel);
        }

        [ClaimsAuthorize("cliente", "n3")]
        [Route("remover-cliente/{id}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _clienteApplication.Remover(id);

            if (!OperacaoValida())
            {
                var clienteViewModel = await _clienteApplication.ObterCliente(id);

                if (clienteViewModel == null) return NotFound();

                return View(clienteViewModel);
            }

            TempData["Sucesso"] = "O cliente foi removido";

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("cliente", "n2")]
        [Route("adicionar-endereco-cliente/{clienteId}")]
        public IActionResult AdicionarEndereco(string clienteId)
        {
            var clienteEnderecoViewModel = new ClienteEnderecoSapViewModel
            {
                ClienteId = clienteId,
                EnderecoExterior = false
            };

            return PartialView("_AdicionarEndereco", clienteEnderecoViewModel);
        }

        [ClaimsAuthorize("cliente", "n2")]
        [Route("adicionar-endereco-cliente/{clienteId}")]
        [HttpPost]
        public async Task<IActionResult> AdicionarEndereco(ClienteEnderecoSapViewModel clienteEnderecoViewModel)
        {
            ModelState.Remove("Identificacao");

            if (!ModelState.IsValid) return PartialView("_AdicionarEndereco", clienteEnderecoViewModel);

            await _clienteApplication.AdicionarEndereco(clienteEnderecoViewModel);

            if (!OperacaoValida()) return PartialView("_AdicionarEndereco", clienteEnderecoViewModel);

            var url = Url.Action("ObterClienteEnderecos", new { id = clienteEnderecoViewModel.ClienteId });
            return Json(new { success = true, url });
        }

        [ClaimsAuthorize("cliente", "n1")]
        [Route("detalhar-endereco-cliente/{clienteId}/{numeroLinha}")]
        public async Task<IActionResult> DetalharEndereco(string clienteId, int numeroLinha)
        {
            var clienteViewModel = await _clienteApplication.ObterCliente(clienteId);

            if (clienteViewModel == null) return NotFound();

            var clienteEnderecoViewModel = clienteViewModel.Enderecos.FirstOrDefault(e => e.NumeroLinha == numeroLinha);

            if (clienteEnderecoViewModel == null) return NotFound();

            return PartialView("_DetalharEndereco", clienteEnderecoViewModel);
        }

        [ClaimsAuthorize("cliente", "n2")]
        [Route("editar-endereco-cliente/{clienteId}/{numeroLinha}")]
        public async Task<IActionResult> EditarEndereco(string clienteId, int numeroLinha)
        {
            var clienteViewModel = await _clienteApplication.ObterCliente(clienteId);

            if (clienteViewModel == null) return NotFound();

            var clienteEnderecoViewModel = clienteViewModel.Enderecos.FirstOrDefault(e => e.NumeroLinha == numeroLinha);

            if (clienteEnderecoViewModel == null) return NotFound();

            clienteEnderecoViewModel.EnderecoExterior = clienteEnderecoViewModel.Cep == "99999999";

            return PartialView("_EditarEndereco", clienteEnderecoViewModel);
        }

        [ClaimsAuthorize("cliente", "n2")]
        [Route("editar-endereco-cliente/{clienteId}/{numeroLinha}")]
        [HttpPost]
        public async Task<IActionResult> EditarEndereco(string clienteId, int numeroLinha, ClienteEnderecoSapViewModel clienteEnderecoViewModel)
        {
            if (clienteId != clienteEnderecoViewModel.ClienteId || numeroLinha != clienteEnderecoViewModel.NumeroLinha) return NotFound();

            if (!ModelState.IsValid) return PartialView("_EditarEndereco", clienteEnderecoViewModel);

            await _clienteApplication.AtualizarEndereco(clienteEnderecoViewModel);

            if (!OperacaoValida()) return PartialView("_EditarEndereco", clienteEnderecoViewModel);

            var url = Url.Action("ObterClienteEnderecos", new { id = clienteId });
            return Json(new { success = true, url });
        }

        [ClaimsAuthorize("cliente", "n2")]
        [Route("adicionar-contato-cliente/{clienteId}")]
        public IActionResult AdicionarContato(string clienteId)
        {
            var clienteContatoViewModel = new ClienteContatoSapViewModel
            {
                ClienteId = clienteId
            };

            return PartialView("_AdicionarContato", clienteContatoViewModel);
        }

        [ClaimsAuthorize("cliente", "n2")]
        [Route("adicionar-contato-cliente/{clienteId}")]
        [HttpPost]
        public async Task<IActionResult> AdicionarContato(ClienteContatoSapViewModel clienteContatoViewModel)
        {
            if (!ModelState.IsValid) return PartialView("_AdicionarContato", clienteContatoViewModel);

            await _clienteApplication.AdicionarContato(clienteContatoViewModel);

            if (!OperacaoValida()) return PartialView("_AdicionarContato", clienteContatoViewModel);

            var url = Url.Action("ObterClienteContatos", new { id = clienteContatoViewModel.ClienteId });
            return Json(new { success = true, url });
        }

        [ClaimsAuthorize("cliente", "n1")]
        [Route("detalhar-contato-cliente/{clienteId}/{id}")]
        public async Task<IActionResult> DetalharContato(string clienteId, int id)
        {
            var clienteViewModel = await _clienteApplication.ObterCliente(clienteId);

            if (clienteViewModel == null) return NotFound();

            var clienteContatoViewModel = clienteViewModel.Contatos.FirstOrDefault(e => e.Id == id);

            if (clienteContatoViewModel == null) return NotFound();

            return PartialView("_DetalharContato", clienteContatoViewModel);
        }

        [ClaimsAuthorize("cliente", "n2")]
        [Route("editar-contato-cliente/{clienteId}/{id}")]
        public async Task<IActionResult> EditarContato(string clienteId, int id)
        {
            var clienteViewModel = await _clienteApplication.ObterCliente(clienteId);

            if (clienteViewModel == null) return NotFound();

            var clienteContatoViewModel = clienteViewModel.Contatos.FirstOrDefault(e => e.Id == id);

            if (clienteContatoViewModel == null) return NotFound();

            return PartialView("_EditarContato", clienteContatoViewModel);
        }

        [ClaimsAuthorize("cliente", "n2")]
        [Route("editar-contato-cliente/{clienteId}/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditarContato(string clienteId, int id, ClienteContatoSapViewModel clienteContatoViewModel)
        {
            if (clienteId != clienteContatoViewModel.ClienteId || id != clienteContatoViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return PartialView("_EditarContato", clienteContatoViewModel);

            await _clienteApplication.AtualizarContato(clienteContatoViewModel);

            if (!OperacaoValida()) return PartialView("_EditarContato", clienteContatoViewModel);

            var url = Url.Action("ObterClienteContatos", new { id = clienteId });
            return Json(new { success = true, url });
        }

        [ClaimsAuthorize("cliente", "n3")]
        [Route("remover-contato-cliente/{clienteId}/{id}")]
        public async Task<IActionResult> RemoverContato(string clienteId, int id)
        {
            var clienteViewModel = await _clienteApplication.ObterCliente(clienteId);

            if (clienteViewModel == null) return NotFound();

            var clienteContatoViewModel = clienteViewModel.Contatos.FirstOrDefault(e => e.Id == id);

            if (clienteContatoViewModel == null) return NotFound();

            return PartialView("_RemoverContato", clienteContatoViewModel);
        }

        [ClaimsAuthorize("cliente", "n3")]
        [Route("remover-contato-cliente/{clienteId}/{id}")]
        [HttpPost, ActionName("RemoverContato")]
        public async Task<IActionResult> RemoverContatoConfirmado(string clienteId, int id)
        {
            await _clienteApplication.RemoverContato(clienteId, id);

            if (!OperacaoValida())
            {
                var clienteViewModel = await _clienteApplication.ObterCliente(clienteId);

                if (clienteViewModel == null) return NotFound();

                var clienteContatoViewModel = clienteViewModel.Contatos.FirstOrDefault(e => e.Id == id);

                if (clienteContatoViewModel == null) return NotFound();

                return PartialView("_RemoverContato", clienteContatoViewModel);
            }

            var url = Url.Action("ObterClienteContatos", new { id = clienteId });
            return Json(new { success = true, url });
        }

        [ClaimsAuthorize("cliente", "n1")]
        [Route("cliente-autocomplete")]
        public async Task<IActionResult> Autocomplete()
        {
            var term = HttpContext.Request.Query["term"].ToString();

            if (term.Length < 4) return Ok();

            var clientes = await _clienteApplication.ObterPorPartNome(term);

            var nomes = clientes.Select(c => c.Nome).OrderBy(c => c).ToList();

            return Ok(nomes);
        }

        [ClaimsAuthorize("cliente", "n1")]
        [Route("cliente-obterdados")]
        public async Task<IActionResult> ObterDados(string nome)
        {
            var clientesViewModel = await _clienteApplication.ObterPorNome(nome);

            if (clientesViewModel.Count != 1) return Json(new { success = false });

            return Json(new { success = true, cliente = JsonConvert.SerializeObject(clientesViewModel[0]) });
        }

        [ClaimsAuthorize("cliente", "n1")]
        public async Task<IActionResult> Visao(string id)
        {
            return PartialView("_Visao", await _clienteApplication.ObterVisao(id));
        }

        public async Task<IActionResult> ObterClienteEnderecos(string id)
        {
            var clienteViewModel = await _clienteApplication.ObterCliente(id);

            if (clienteViewModel == null) return NotFound();

            return PartialView("_ListaEnderecos", clienteViewModel);
        }

        public async Task<IActionResult> ObterClienteContatos(string id)
        {
            var clienteViewModel = await _clienteApplication.ObterCliente(id);

            if (clienteViewModel == null) return NotFound();

            return PartialView("_ListaContatos", clienteViewModel);
        }

        public async Task<IActionResult> LimparFiltrosPesquisa(string cpfFilter, string cnpjFilter, string nomeFilter)
        {
            CleanSearchFilters(await GerarFiltrosPesquisas(cpfFilter, cnpjFilter, nomeFilter));

            return RedirectToAction("Index");
        }

        private Task<List<SearchFilter>> GerarFiltrosPesquisas(string cpfFilter, string cnpjFilter, string nomeFilter)
        {
            return Task.FromResult(new List<SearchFilter>
            {
                new SearchFilter { KeyFilter = "ClienteCpfFilter", ValueFilter = cpfFilter },
                new SearchFilter { KeyFilter = "ClienteCnpjFilter", ValueFilter = cnpjFilter },
                new SearchFilter { KeyFilter = "ClienteNomeFilter", ValueFilter = nomeFilter }
            });
        }
    }
}