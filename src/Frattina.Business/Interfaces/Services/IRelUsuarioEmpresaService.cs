using Frattina.Business.Models;
using System;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Services
{
    public interface IRelUsuarioEmpresaService : IDisposable
    {
        Task Adicionar(RelUsuarioEmpresa relUsuarioEmpresa);

        Task Remover(Guid id);
    }
}
