using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IProdutoTipoApplication : IDisposable
    {
        Task<ProdutoTipoSapViewModel> ObterTipo(int id);

        Task<List<ProdutoTipoSapViewModel>> ObterTodos();
    }
}
