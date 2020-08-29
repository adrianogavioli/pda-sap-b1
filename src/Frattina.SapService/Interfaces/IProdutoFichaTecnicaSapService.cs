using Frattina.SapService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.SapService.Interfaces
{
    public interface IProdutoFichaTecnicaSapService : IDisposable
    {
        Task<IEnumerable<ProdutoFichaTecnicaSap>> ObterPorItemCode(string itemCode);
    }
}
