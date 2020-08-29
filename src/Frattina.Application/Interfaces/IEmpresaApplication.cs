using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IEmpresaApplication : IDisposable
    {
        Task<EmpresaSapViewModel> ObterEmpresa(int id);

        Task<List<EmpresaSapViewModel>> ObterTodos();
    }
}
