using Frattina.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Repositories
{
    public interface IAtendimentoProdutoRepository : IRepository<AtendimentoProduto>
    {
        Task<AtendimentoProduto> ObterAtendimentoProduto(Guid id);

        Task<IEnumerable<AtendimentoProduto>> ObterAtendimentoProdutosPorAtendimento(Guid atendimentoId);

        Task<IEnumerable<AtendimentoProduto>> ObterAtendimentoProdutosPorProduto(string ProdutoId);

        Task<IEnumerable<AtendimentoProduto>> ObterAtendimentoProdutosAmadosPorCliente(string clienteId);

        Task<IEnumerable<AtendimentoProduto>> ObterAtendimentoProdutosAmados();

        Task<IEnumerable<AtendimentoProduto>> ObterAtendimentoProdutosNaoAmados();
    }
}
