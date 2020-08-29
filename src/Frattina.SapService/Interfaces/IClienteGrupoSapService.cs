using Frattina.SapService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.SapService.Interfaces
{
    public interface IClienteGrupoSapService : IDisposable
    {
        Task<ClienteGrupoSap> ObterGrupo(int code);

        Task<IEnumerable<ClienteGrupoSap>> ObterTodos();
    }
}
