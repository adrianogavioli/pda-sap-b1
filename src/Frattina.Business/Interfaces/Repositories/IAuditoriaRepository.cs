using Frattina.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Repositories
{
    public interface IAuditoriaRepository : IDisposable
    {
        Task Adicionar(Auditoria auditoria);

        Task<Auditoria> ObterAuditoria(Guid id);

        Task<IEnumerable<Auditoria>> ObterAuditorias(string tabela, Guid chave);
    }
}
