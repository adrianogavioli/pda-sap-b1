using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Models;
using Frattina.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Frattina.Data.Repositories
{
    public class CargoRepository : Repository<Cargo>, ICargoRepository
    {
        public CargoRepository(FrattinaDbContext context) : base(context) { }

        public async Task<Cargo> ObterCargo(Guid id)
        {
            return await Db.Cargos
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cargo> ObterCargoUsuarios(Guid id)
        {
            return await Db.Cargos
                .AsNoTracking()
                .Include(c => c.Usuarios)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
