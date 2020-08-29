using Frattina.SapService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.SapService.Interfaces
{
    public interface IProdutoTipoSapService : IDisposable
    {
        Task<ProdutoTipoSap> ObterTipo(int code);

        Task<IEnumerable<ProdutoTipoSap>> ObterTodos();
    }
}
