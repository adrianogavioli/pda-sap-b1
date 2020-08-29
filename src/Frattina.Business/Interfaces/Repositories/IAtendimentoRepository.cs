using Frattina.Business.Models;
using Frattina.Business.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Repositories
{
    public interface IAtendimentoRepository : IRepository<Atendimento>
    {
        Task<Atendimento> ObterAtendimento(Guid id);

        Task<Atendimento> ObterAtendimentoProdutosTarefas(Guid id);

        Task<Atendimento> ObterAtendimentoProdutosTarefasAuditoria(Guid id);

        Task<IEnumerable<Atendimento>> ObterAtendimentosProdutosTarefas();

        Task<IEnumerable<Atendimento>> ObterAtendimentosProdutosTarefasPorEtapa(AtendimentoEtapa etapa);

        Task<IEnumerable<Atendimento>> ObterAtendimentosProdutosTarefasPorVendedor(int vendedorId);

        Task<IEnumerable<Atendimento>> ObterAtendimentosProdutosTarefasPorEmpresa(int empresaId);

        Task<IEnumerable<Atendimento>> ObterAtendimentosProdutosTarefasPorCliente(string clienteId);

        Task<IEnumerable<(string Vendedor, int QtdAtendimento, int QtdVenda)>> ObterAtendimentosAgrupadosPorVendedores();
    }
}
