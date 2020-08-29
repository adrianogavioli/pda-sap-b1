using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IProdutoGrupoApplication : IDisposable
    {
        Task<ProdutoGrupoSapViewModel> ObterGrupo(int id);

        Task<List<ProdutoGrupoSapViewModel>> ObterTodos();
    }
}
