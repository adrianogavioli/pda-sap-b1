using Frattina.SapService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.SapService.Interfaces
{
    public interface IClienteSapService : IDisposable
    {
        Task<ClienteSap> Adicionar(ClienteSap cliente);

        Task Atualizar(ClienteSap cliente);

        Task Remover(string cardCode);

        Task AdicionarEndereco(ClienteEnderecoSap clienteEndereco);

        Task AtualizarEndereco(ClienteEnderecoSap clienteEndereco);

        Task AdicionarContato(ClienteContatoSap clienteContato);

        Task AtualizarContato(ClienteContatoSap clienteContato);

        Task RemoverContato(string cardCode, int internalCode);

        Task<ClienteSap> ObterCliente(string cardCode);

        Task<ClienteSap> ObterPorCPF(string cpf);

        Task<ClienteSap> ObterPorCNPJ(string cnpj);

        Task<IEnumerable<ClienteSap>> ObterPorCardName(string cardName);

        Task<IEnumerable<ClienteSap>> ObterPorPartCardName(string cardName);
    }
}
