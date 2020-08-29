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
    public class UsuarioProdutoConsultaRepository : Repository<UsuarioProdutoConsulta>, IUsuarioProdutoConsultaRepository
    {
        public UsuarioProdutoConsultaRepository(FrattinaDbContext context) : base(context) { }

        public async Task<IEnumerable<UsuarioProdutoConsulta>> ObterUltimasVinteConsultasPorUsuario(Guid usuarioId)
        {
            return await DbSet
                .AsNoTracking()
                .Where(c => c.UsuarioId == usuarioId)
                .OrderByDescending(c => c.DataCadastro)
                .Take(20)
                .ToListAsync();
        }
    }
}
