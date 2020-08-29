using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IClienteApplication : IDisposable
    {
        Task<ClienteSapViewModel> Adicionar(ClienteSapViewModel clienteSapViewModel);

        Task Atualizar(ClienteSapViewModel clienteSapViewModel);

        Task Remover(string id);

        Task AdicionarEndereco(ClienteEnderecoSapViewModel clienteEnderecoSapViewModel);

        Task AtualizarEndereco(ClienteEnderecoSapViewModel clienteEnderecoSapViewModel);

        Task AdicionarContato(ClienteContatoSapViewModel clienteContatoSapViewModel);

        Task AtualizarContato(ClienteContatoSapViewModel clienteContatoSapViewModel);

        Task RemoverContato(string clienteId, int contatoId);

        Task<ClienteSapViewModel> ObterCliente(string id);

        Task<ClienteSapViewModel> ObterPorCPF(string cpf);

        Task<ClienteSapViewModel> ObterPorCNPJ(string cnpj);

        Task<List<ClienteSapViewModel>> ObterPorNome(string nome);

        Task<List<ClienteSapViewModel>> ObterPorPartNome(string nome);

        Task<ClienteVisaoViewModel> ObterVisao(string id);
    }
}
