using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Models;
using Frattina.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frattina.Business.Models.Enums;

namespace Frattina.Data.Repositories
{
    public class AtendimentoProdutoRepository : Repository<AtendimentoProduto>, IAtendimentoProdutoRepository
    {
        public AtendimentoProdutoRepository(FrattinaDbContext context) : base(context) { }

        public async Task<AtendimentoProduto> ObterAtendimentoProduto(Guid id)
        {
            return await Db.AtendimentosProdutos
                .AsNoTracking()
                .Include(a => a.Atendimento)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<AtendimentoProduto>> ObterAtendimentoProdutosPorAtendimento(Guid atendimentoId)
        {
            return await Db.AtendimentosProdutos
                .AsNoTracking()
                .Where(p => p.AtendimentoId == atendimentoId && !p.RemovidoAtendimento)
                .ToListAsync();
        }

        public async Task<IEnumerable<AtendimentoProduto>> ObterAtendimentoProdutosAmadosPorCliente(string clienteId)
        {
            return await Db.AtendimentosProdutos
                .AsNoTracking()
                .Where(p => p.Atendimento.ClienteId == clienteId
                    && p.NivelInteresse == 3
                    && !p.RemovidoAtendimento
                    && (
                            p.Atendimento.Etapa != AtendimentoEtapa.Vendido
                            || p.Atendimento.Etapa == AtendimentoEtapa.Vendido && p.RemovidoVenda
                        )
                    )
                .ToListAsync();
        }

        public async Task<IEnumerable<AtendimentoProduto>> ObterAtendimentoProdutosPorProduto(string ProdutoId)
        {
            return await Db.AtendimentosProdutos
                .AsNoTracking()
                .Include(p => p.Atendimento)
                .Where(p => p.ProdutoSapId == ProdutoId
                    && !p.RemovidoAtendimento)
                .ToListAsync();
        }

        public async Task<IEnumerable<AtendimentoProduto>> ObterAtendimentoProdutosAmados()
        {
            return await Db.AtendimentosProdutos
                .AsNoTracking()
                .Where(p => p.NivelInteresse == 3
                    && !p.RemovidoAtendimento)
                .ToListAsync();
        }

        public async Task<IEnumerable<AtendimentoProduto>> ObterAtendimentoProdutosNaoAmados()
        {
            return await Db.AtendimentosProdutos
                .AsNoTracking()
                .Where(p => p.NivelInteresse == 1
                    && !p.RemovidoAtendimento)
                .ToListAsync();
        }
    }
}
