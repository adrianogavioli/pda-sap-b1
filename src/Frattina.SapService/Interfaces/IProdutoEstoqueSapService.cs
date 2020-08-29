using Frattina.SapService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.SapService.Interfaces
{
    public interface IProdutoEstoqueSapService : IDisposable
    {
        Task<IEnumerable<ProdutoEstoqueSap>> ObterPorItemCode(string itemCode);
    }
}
