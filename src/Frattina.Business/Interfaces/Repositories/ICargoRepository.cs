using Frattina.Business.Models;
using System;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Repositories
{
    public interface ICargoRepository : IRepository<Cargo>
    {
        Task<Cargo> ObterCargo(Guid id);

        Task<Cargo> ObterCargoUsuarios(Guid id);
    }
}
