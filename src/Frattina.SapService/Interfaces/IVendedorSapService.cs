using Frattina.SapService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.SapService.Interfaces
{
    public interface IVendedorSapService : IDisposable
    {
        Task<VendedorSap> ObterVendedor(int SalesEmployeeCode);

        Task<IEnumerable<VendedorSap>> ObterTodos();
    }
}
