using Frattina.Business.Models;
using System;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Services
{
    public interface IAuditoriaService : IDisposable
    {
        Task Adicionar(Auditoria auditoria);
    }
}
