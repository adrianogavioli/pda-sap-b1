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
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly FrattinaDbContext _frattinaDb;

        public AuditoriaRepository(FrattinaDbContext frattinaDb)
        {
            _frattinaDb = frattinaDb;
        }

        public async Task Adicionar(Auditoria auditoria)
        {
            _frattinaDb.Auditorias.Add(auditoria);

            await _frattinaDb.SaveChangesAsync();
        }

        public async Task<Auditoria> ObterAuditoria(Guid id)
        {
            return await _frattinaDb.Auditorias
                .AsNoTracking()
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Auditoria>> ObterAuditorias(string tabela, Guid chave)
        {
            return await _frattinaDb.Auditorias
                .AsNoTracking()
                .Include(a => a.Usuario)
                .Where(a => a.Tabela == tabela && a.Chave == chave)
                .ToListAsync();
        }

        public void Dispose()
        {
            _frattinaDb.Dispose();
        }
    }
}
