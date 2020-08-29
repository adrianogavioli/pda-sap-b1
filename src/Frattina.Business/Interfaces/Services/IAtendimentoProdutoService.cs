using Frattina.Business.Models;
using System;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Services
{
    public interface IAtendimentoProdutoService : IDisposable
    {
        Task Adicionar(AtendimentoProduto atendimentoProduto);

        Task AtualizarValorNegociado(AtendimentoProduto atendimentoProduto);

        Task AtualizarNivelInteresse(AtendimentoProduto atendimentoProduto);

        Task RemoverDoAtendimento(AtendimentoProduto atendimentoProduto);

        Task RemoverDaVenda(AtendimentoProduto atendimentoProduto);

        Task AdicionarAVenda(AtendimentoProduto atendimentoProduto);
    }
}
