using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Models;
using Frattina.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Frattina.Data.Repositories
{
    public class AtendimentoEncerradoRepository : Repository<AtendimentoEncerrado>, IAtendimentoEncerradoRepository
    {
        public AtendimentoEncerradoRepository(FrattinaDbContext context) : base(context) { }

        public async Task<AtendimentoEncerrado> ObterAtendimentoEncerrado(Guid id)
        {
            return await Db.AtendimentosEncerrados
                .AsNoTracking()
                .Include(a => a.Atendimento)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
