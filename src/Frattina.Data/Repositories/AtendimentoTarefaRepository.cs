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
    public class AtendimentoTarefaRepository : Repository<AtendimentoTarefa>, IAtendimentoTarefaRepository
    {
        public AtendimentoTarefaRepository(FrattinaDbContext context) : base(context) { }

        public async Task<AtendimentoTarefa> ObterAtendimentoTarefa(Guid id)
        {
            return await Db.AtendimentosTarefas
                .AsNoTracking()
                .Include(t => t.Atendimento)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<AtendimentoTarefa>> ObterAtendimentoTarefasPorAtendimento(Guid atendimentoId)
        {
            return await Db.AtendimentosTarefas
                .AsNoTracking()
                .Where(t => t.AtendimentoId == atendimentoId && !t.Removida)
                .ToListAsync();
        }

        public async Task<IEnumerable<AtendimentoTarefa>> ObterAtendimentoTarefasPorVendedor(int vendedorId)
        {
            return await Db.AtendimentosTarefas
                .AsNoTracking()
                .Where(t => t.Atendimento.VendedorId == vendedorId && !t.Removida)
                .ToListAsync();
        }

        public async Task<IEnumerable<AtendimentoTarefa>> ObterAtendimentoTarefasAtivas()
        {
            return await Db.AtendimentosTarefas
                .AsNoTracking()
                .Include(t => t.Atendimento)
                .Where(t => t.DataFinalizacao == null && !t.Removida)
                .ToListAsync();
        }

        public async Task<IEnumerable<AtendimentoTarefa>> ObterAtendimentoTarefasAtrasadas()
        {
            return await Db.AtendimentosTarefas
                .AsNoTracking()
                .Include(t => t.Atendimento)
                .Where(t => t.DataTarefa < DateTime.Now
                    && t.DataFinalizacao == null
                    && !t.Removida)
                .ToListAsync();
        }
    }
}
