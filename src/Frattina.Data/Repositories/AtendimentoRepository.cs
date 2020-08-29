using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Models;
using Frattina.Business.Models.Enums;
using Frattina.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.Data.Repositories
{
    public class AtendimentoRepository : Repository<Atendimento>, IAtendimentoRepository
    {
        public AtendimentoRepository(FrattinaDbContext context) : base(context) { }

        public async Task<Atendimento> ObterAtendimento(Guid id)
        {
            return await Db.Atendimentos
                .AsNoTracking()
                .Include(a => a.Encerrado)
                .Include(a => a.Vendido)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Atendimento> ObterAtendimentoProdutosTarefas(Guid id)
        {
            var atendimento = await Db.Atendimentos
                .AsNoTracking()
                .Include(a => a.Encerrado)
                .Include(a => a.Vendido)
                .Include(a => a.Produtos)
                .Include(a => a.Tarefas)
                .FirstOrDefaultAsync(a => a.Id == id);

            await RemoverProdutosRemovidos(atendimento);

            await RemoverTarefasRemovidas(atendimento);

            return atendimento;
        }

        public async Task<Atendimento> ObterAtendimentoProdutosTarefasAuditoria(Guid id)
        {
            return await Db.Atendimentos
                .AsNoTracking()
                .Include(a => a.Encerrado)
                .Include(a => a.Vendido)
                .Include(a => a.Produtos)
                .Include(a => a.Tarefas)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Atendimento>> ObterAtendimentosProdutosTarefas()
        {
            var atendimentos = await Db.Atendimentos
                .AsNoTracking()
                .Include(a => a.Produtos)
                .Include(a => a.Tarefas)
                .ToListAsync();

            foreach (var atendimento in atendimentos)
            {
                await RemoverProdutosRemovidos(atendimento);

                await RemoverTarefasRemovidas(atendimento);
            }

            return atendimentos;
        }

        public async Task<IEnumerable<Atendimento>> ObterAtendimentosProdutosTarefasPorEtapa(AtendimentoEtapa etapa)
        {
            var atendimentos = await Db.Atendimentos
                .AsNoTracking()
                .Include(a => a.Produtos)
                .Include(a => a.Tarefas)
                .Where(a => a.Etapa == etapa)
                .ToListAsync();

            foreach (var atendimento in atendimentos)
            {
                await RemoverProdutosRemovidos(atendimento);

                await RemoverTarefasRemovidas(atendimento);
            }

            return atendimentos;
        }

        public async Task<IEnumerable<Atendimento>> ObterAtendimentosProdutosTarefasPorVendedor(int vendedorId)
        {
            var atendimentos = await Db.Atendimentos
                .AsNoTracking()
                .Include(a => a.Produtos)
                .Include(a => a.Tarefas)
                .Where(a => a.VendedorId == vendedorId)
                .ToListAsync();

            foreach (var atendimento in atendimentos)
            {
                await RemoverProdutosRemovidos(atendimento);

                await RemoverTarefasRemovidas(atendimento);
            }

            return atendimentos;
        }

        public async Task<IEnumerable<Atendimento>> ObterAtendimentosProdutosTarefasPorEmpresa(int empresaId)
        {
            var atendimentos = await Db.Atendimentos
                .AsNoTracking()
                .Include(a => a.Produtos)
                .Include(a => a.Tarefas)
                .Where(a => a.EmpresaId == empresaId)
                .ToListAsync();

            foreach (var atendimento in atendimentos)
            {
                await RemoverProdutosRemovidos(atendimento);

                await RemoverTarefasRemovidas(atendimento);
            }

            return atendimentos;
        }

        public async Task<IEnumerable<Atendimento>> ObterAtendimentosProdutosTarefasPorCliente(string clienteId)
        {
            var atendimentos = await Db.Atendimentos
                .AsNoTracking()
                .Include(a => a.Produtos)
                .Include(a => a.Tarefas)
                .Where(a => a.ClienteId == clienteId)
                .ToListAsync();

            foreach (var atendimento in atendimentos)
            {
                await RemoverProdutosRemovidos(atendimento);

                await RemoverTarefasRemovidas(atendimento);
            }

            return atendimentos;
        }

        public async Task<IEnumerable<(string Vendedor, int QtdAtendimento, int QtdVenda)>> ObterAtendimentosAgrupadosPorVendedores()
        {
            var atendimentos = await Db.Atendimentos
                .AsNoTracking()
                .GroupBy(a => a.VendedorId)
                .Select(a => new
                {
                    Vendedor = a.FirstOrDefault().VendedorNome,
                    QtdAtendimento = a.Count(),
                    QtdVenda = a.Where(w => w.Etapa == AtendimentoEtapa.Vendido).Count()
                })
                .ToListAsync();

            return atendimentos.Select(a => (
                a.Vendedor,
                a.QtdAtendimento,
                a.QtdVenda
            ));
        }

        private Task<Atendimento> RemoverProdutosRemovidos(Atendimento atendimento)
        {
            atendimento.Produtos = atendimento.Produtos.Where(p => !p.RemovidoAtendimento);

            return Task.FromResult(atendimento);
        }

        private Task<Atendimento> RemoverTarefasRemovidas(Atendimento atendimento)
        {
            atendimento.Tarefas = atendimento.Tarefas.Where(t => !t.Removida);

            return Task.FromResult(atendimento);
        }
    }
}
