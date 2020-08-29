using Frattina.Business.Models;
using System;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Services
{
    public interface ICargoService : IDisposable
    {
        Task Adicionar(Cargo cargo);

        Task Atualizar(Cargo cargo);

        Task Remover(Guid id);
    }
}
