using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IProdutoModeloApplication : IDisposable
    {
        Task<ProdutoModeloSapViewModel> ObterModelo(int id);

        Task<List<ProdutoModeloSapViewModel>> ObterPorTipo(int tipoId);

        Task<List<ProdutoModeloSapViewModel>> ObterPorMarca(int marcaId);

        Task<List<ProdutoModeloSapViewModel>> ObterPorTipoMarca(int tipoId, int marcaId);

        Task<List<ProdutoModeloSapViewModel>> ObterTodos();
    }
}
