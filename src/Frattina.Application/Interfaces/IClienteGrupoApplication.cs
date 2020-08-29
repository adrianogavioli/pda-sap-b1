using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IClienteGrupoApplication : IDisposable
    {
        Task<ClienteGrupoSapViewModel> ObterGrupo(int id);

        Task<List<ClienteGrupoSapViewModel>> ObterTodos();
    }
}
