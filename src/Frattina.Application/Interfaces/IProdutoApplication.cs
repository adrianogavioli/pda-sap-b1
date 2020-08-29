using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IProdutoApplication : IDisposable
    {
        Task<ProdutoSapViewModel> ObterProduto(string id);

        Task<List<ProdutoSapViewModel>> ObterPorCodigoMaster(string codigoMaster);

        Task<List<ProdutoSapViewModel>> ObterPorTipo(int tipoId);

        Task<List<ProdutoSapViewModel>> ObterPorMarca(int marcaId);

        Task<List<ProdutoSapViewModel>> ObterPorModelo(int modeloId);

        Task<List<ProdutoSapViewModel>> ObterPorReferencia(string referencia);

        Task<List<ProdutoSapViewModel>> ObterPorTipoMarcaModelo(int? tipoId, int? marcaId, int? modeloId);

        Task<ProdutoVisaoViewModel> ObterVisao(string id);
    }
}
