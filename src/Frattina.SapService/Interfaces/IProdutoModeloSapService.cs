using Frattina.SapService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.SapService.Interfaces
{
    public interface IProdutoModeloSapService : IDisposable
    {
        Task<ProdutoModeloSap> ObterModelo(int code);

        Task<IEnumerable<ProdutoModeloSap>> ObterPorTipo(int tipoCode);

        Task<IEnumerable<ProdutoModeloSap>> ObterPorMarca(int marcaCode);

        Task<IEnumerable<ProdutoModeloSap>> ObterPorTipoMarca(int tipoCode, int marcaCode);

        Task<IEnumerable<ProdutoModeloSap>> ObterTodos();
    }
}
