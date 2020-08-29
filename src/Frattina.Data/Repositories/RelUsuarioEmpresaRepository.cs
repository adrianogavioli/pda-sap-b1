using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Models;
using Frattina.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.Data.Repositories
{
    public class RelUsuarioEmpresaRepository : Repository<RelUsuarioEmpresa>, IRelUsuarioEmpresaRepository
    {
        public RelUsuarioEmpresaRepository(FrattinaDbContext context) : base(context) { }

        public async Task<RelUsuarioEmpresa> ObterRelUsuarioEmpresa(Guid id)
        {
            return await DbSet
                .AsNoTracking()
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<RelUsuarioEmpresa>> ObterRelUsuarioEmpresaPorUsuario(Guid usuarioId)
        {
            return await DbSet
                .AsNoTracking()
                .Include(r => r.Usuario)
                .Where(r => r.UsuarioId == usuarioId && !r.Removido)
                .ToListAsync();
        }

        public async Task<IEnumerable<RelUsuarioEmpresa>> ObterRelUsuarioEmpresaPorEmpresa(int empresaId)
        {
            return await DbSet
                .AsNoTracking()
                .Include(r => r.Usuario)
                .Where(r => r.EmpresaId == empresaId && !r.Removido)
                .ToListAsync();
        }
    }
}
