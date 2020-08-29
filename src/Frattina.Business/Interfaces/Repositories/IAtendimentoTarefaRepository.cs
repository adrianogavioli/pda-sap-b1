using Frattina.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Repositories
{
    public interface IAtendimentoTarefaRepository : IRepository<AtendimentoTarefa>
    {
        Task<AtendimentoTarefa> ObterAtendimentoTarefa(Guid id);

        Task<IEnumerable<AtendimentoTarefa>> ObterAtendimentoTarefasPorAtendimento(Guid atendimentoId);

        Task<IEnumerable<AtendimentoTarefa>> ObterAtendimentoTarefasPorVendedor(int vendedorId);

        Task<IEnumerable<AtendimentoTarefa>> ObterAtendimentoTarefasAtivas();

        Task<IEnumerable<AtendimentoTarefa>> ObterAtendimentoTarefasAtrasadas();
    }
}
