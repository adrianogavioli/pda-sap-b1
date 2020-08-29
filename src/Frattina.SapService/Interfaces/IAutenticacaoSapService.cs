using System;
using System.Threading.Tasks;

namespace Frattina.SapService.Interfaces
{
    public interface IAutenticacaoSapService : IDisposable
    {
        Task<bool> Login();

        Task Logout();
    }
}
