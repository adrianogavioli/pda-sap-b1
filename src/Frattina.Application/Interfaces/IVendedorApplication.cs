using Frattina.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IVendedorApplication : IDisposable
    {
        Task<VendedorSapViewModel> ObterVendedor(int id);

        Task<List<VendedorSapViewModel>> ObterTodos();

        Task<VendedorVisaoViewModel> ObterVisao(int id);
    }
}
