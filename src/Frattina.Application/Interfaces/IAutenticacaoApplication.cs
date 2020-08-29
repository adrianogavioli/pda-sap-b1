using System;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IAutenticacaoApplication : IDisposable
    {
        Task<bool> Login();

        Task Logout();
    }
}
