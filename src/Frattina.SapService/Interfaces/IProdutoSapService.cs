using Frattina.SapService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.SapService.Interfaces
{
    public interface IProdutoSapService : IDisposable
    {
        Task<ProdutoSap> ObterProduto(string itemCode);

        Task<IEnumerable<ProdutoSap>> ObterPorMasterCode(string masterCode);

        Task<IEnumerable<ProdutoSap>> ObterPorTipoCode(int code);

        Task<IEnumerable<ProdutoSap>> ObterPorMarcaCode(int code);

        Task<IEnumerable<ProdutoSap>> ObterPorModeloCode(int code);

        Task<IEnumerable<ProdutoSap>> ObterPorSupplierCatalog(string supplierCatalog);

        Task<IEnumerable<ProdutoSap>> ObterPorTipoMarcaModeloCodes(int? tipoCode, int? marcaCode, int? modeloCode);
    }
}
