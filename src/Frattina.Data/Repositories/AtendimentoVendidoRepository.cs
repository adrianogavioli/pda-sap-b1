using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Models;
using Frattina.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Frattina.Data.Repositories
{
    public class AtendimentoVendidoRepository : Repository<AtendimentoVendido>, IAtendimentoVendidoRepository
    {
        public AtendimentoVendidoRepository(FrattinaDbContext context) : base(context) { }

        public async Task<AtendimentoVendido> ObterAtendimentoVendido(Guid id)
        {
            return await Db.AtendimentosVendidos
                .AsNoTracking()
                .Include(a => a.Atendimento)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
