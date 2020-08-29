using Frattina.SapService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.SapService.Interfaces
{
    public interface IProdutoMarcaSapService : IDisposable
    {
        Task<ProdutoMarcaSap> ObterMarca(int code);

        Task<IEnumerable<ProdutoMarcaSap>> ObterPorTipo(int tipoCode);

        Task<IEnumerable<ProdutoMarcaSap>> ObterTodos();
    }
}
