using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IEstoqueApplication : IDisposable
    {
        Task<List<EstoqueSapViewModel>> ObterPorEmpresa(int EmpresaId);

        Task<List<EstoqueSapViewModel>> ObterTodos();
    }
}
