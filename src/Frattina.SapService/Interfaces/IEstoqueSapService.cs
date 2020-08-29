using Frattina.SapService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.SapService.Interfaces
{
    public interface IEstoqueSapService : IDisposable
    {
        Task<IEnumerable<EstoqueSap>> ObterPorEmpresa(int BusinessPlaceID);

        Task<IEnumerable<EstoqueSap>> ObterTodos();
    }
}
