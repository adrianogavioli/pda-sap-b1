using Frattina.SapService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.SapService.Interfaces
{
    public interface IUsuarioSapService : IDisposable
    {
        Task<UsuarioSap> ObterUsuario(int internalKey);

        Task<IEnumerable<UsuarioSap>> ObterTodos();
    }
}
