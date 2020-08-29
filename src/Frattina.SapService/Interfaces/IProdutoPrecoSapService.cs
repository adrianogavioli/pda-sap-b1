using Frattina.SapService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.SapService.Interfaces
{
    public interface IProdutoPrecoSapService : IDisposable
    {
        Task<IEnumerable<ProdutoPrecoSap>> ObterPorItemCode(string itemCode);
    }
}
