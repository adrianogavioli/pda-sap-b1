using Frattina.SapService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.SapService.Interfaces
{
    public interface IProdutoGrupoSapService : IDisposable
    {
        Task<ProdutoGrupoSap> ObterGrupo(int Number);

        Task<IEnumerable<ProdutoGrupoSap>> ObterTodos();
    }
}
