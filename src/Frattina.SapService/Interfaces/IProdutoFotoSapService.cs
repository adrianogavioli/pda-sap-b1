using Frattina.SapService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.SapService.Interfaces
{
    public interface IProdutoFotoSapService : IDisposable
    {
        Task<ProdutoFotoSap> ObterFoto(int code);

        Task<IEnumerable<ProdutoFotoSap>> ObterPorItemCode(string itemCode);
    }
}
