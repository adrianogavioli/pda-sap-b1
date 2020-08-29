using Frattina.SapService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.SapService.Interfaces
{
    public interface IEmpresaSapService : IDisposable
    {
        Task<EmpresaSap> ObterEmpresa(int BPLID);

        Task<IEnumerable<EmpresaSap>> ObterTodos();
    }
}
