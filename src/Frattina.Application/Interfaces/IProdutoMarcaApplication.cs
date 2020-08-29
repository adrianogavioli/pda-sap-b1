using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IProdutoMarcaApplication : IDisposable
    {
        Task<ProdutoMarcaSapViewModel> ObterMarca(int id);

        Task<List<ProdutoMarcaSapViewModel>> ObterPorTipo(int tipoId);

        Task<List<ProdutoMarcaSapViewModel>> ObterTodos();
    }
}
