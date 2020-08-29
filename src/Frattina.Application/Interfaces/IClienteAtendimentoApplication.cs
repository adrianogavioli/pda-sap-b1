using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IClienteAtendimentoApplication : IDisposable
    {
        Task<ClienteVisaoViewModel> PopularVisaoClienteAtendimentos(ClienteVisaoViewModel clienteVisaoViewModel);

        Task<ClienteVisaoViewModel> PopularVisaoClientePoderCompra(ClienteVisaoViewModel clienteVisaoViewModel);

        Task<ClienteVisaoViewModel> PopularVisaoClienteProdutosAmados(ClienteVisaoViewModel clienteVisaoViewModel);

        Task<AtendimentoViewModel> GerenciarClienteAtendimento(AtendimentoViewModel atendimentoViewModel);

        Task<List<AtendimentoProdutoViewModel>> ObterAtendimentoProdutosAmadosPorCliente(string clienteId, bool removerDuplicados);
    }
}
