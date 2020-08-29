using Frattina.App.Dashboard.ViewModels;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Models.Enums;
using Frattina.CrossCutting.Matematica;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.App.Dashboard
{
    public class ElementDashboard : IElementDashboard
    {
        private readonly IAtendimentoApplication _atendimentoApplication;

        public ElementDashboard(IAtendimentoApplication atendimentoApplication)
        {
            _atendimentoApplication = atendimentoApplication;
        }

        public async Task<GraficoTaxaConversaoVendaViewModel> ObterGraficoTaxaConversaoVenda()
        {
            var dadosGrafico = new List<string>
            {
                "['vendedor', 'taxa de conversão']"
            };

            var atendimentosPorVendedoresViewModel = await ObterTabelaAtendimentosPorVendedores();

            atendimentosPorVendedoresViewModel.ForEach(x => dadosGrafico.Add($"['{x.Vendedor}', {x.TaxaConversaoVenda.ToString("n1").Replace(".", "").Replace(",", ".")}]"));

            return new GraficoTaxaConversaoVendaViewModel
            {
                DadosGrafico = string.Join(",", dadosGrafico)
            };
        }

        public async Task<List<TabelaAtendimentosPorVendedoresViewModel>> ObterTabelaAtendimentosPorVendedores()
        {
            var atendimentos = await _atendimentoApplication.ObterAtendimentosAgrupadosPorVendedores();

            var tabelaAtendimentosPorVendedores = new List<TabelaAtendimentosPorVendedoresViewModel>();

            foreach (var atendimento in atendimentos)
            {
                var taxaConversao = Calculos.CalcularTaxaConversao(Convert.ToDecimal(atendimento.QtdVenda), Convert.ToDecimal(atendimento.QtdAtendimento));

                tabelaAtendimentosPorVendedores.Add(new TabelaAtendimentosPorVendedoresViewModel
                {
                    Vendedor = atendimento.Vendedor,
                    QtdAtendimento = atendimento.QtdAtendimento,
                    QtdVenda = atendimento.QtdVenda,
                    TaxaConversaoVenda = taxaConversao
                });
            }

            return tabelaAtendimentosPorVendedores.OrderByDescending(a => a.TaxaConversaoVenda).ToList();
        }

        public async Task<List<AtendimentoTarefaViewModel>> ObterTabelaTarefasFuturas(UsuarioViewModel usuarioViewModel)
        {
            var tarefasAtivas = await _atendimentoApplication.ObterAtendimentoTarefasAtivas();

            var tarefasFuturas = new List<AtendimentoTarefaViewModel>();

            if (usuarioViewModel.Tipo == UsuarioTipo.ADMINISTRADOR || usuarioViewModel.Tipo == UsuarioTipo.GERENTE)
            {
                tarefasFuturas.AddRange(tarefasAtivas.Where(t => t.DataTarefa < DateTime.Today.AddDays(7)));
            }
            else if (usuarioViewModel.Tipo == UsuarioTipo.VENDEDOR && usuarioViewModel.VendedorSapId != null)
            {
                tarefasFuturas.AddRange(tarefasAtivas.Where(t => t.DataTarefa < DateTime.Today.AddDays(7)
                                        && t.Atendimento.VendedorId == Convert.ToInt32(usuarioViewModel.VendedorSapId)));
            }

            return tarefasFuturas.OrderBy(t => t.DataTarefa).ToList();
        }

        public async Task<List<GaleriaTopDezProdutosAmadosViewModel>> ObterGaleriaTopDezProdutosAmados()
        {
            var produtosAmados = await _atendimentoApplication.ObterAtendimentoProdutosAmados();

            return produtosAmados
                .GroupBy(p => new { p.ProdutoSapId })
                .Select(p => new
                {
                    p.FirstOrDefault().ProdutoSapId,
                    p.FirstOrDefault().Descricao,
                    p.FirstOrDefault().Imagem,
                    Quantidade = p.Count()
                })
                .OrderByDescending(p => p.Quantidade)
                .Take(10)
                .ToList()
                .ConvertAll(p => new GaleriaTopDezProdutosAmadosViewModel
                {
                    ProdutoSapId = p.ProdutoSapId,
                    Descricao = p.Descricao,
                    Imagem = p.Imagem,
                    Quantidade = p.Quantidade
                });
        }

        public async Task<List<GaleriaTopDezProdutosNaoAmadosViewModel>> ObterGaleriaTopDezProdutosNaoAmados()
        {
            var produtosNaoAmados = await _atendimentoApplication.ObterAtendimentoProdutosNaoAmados();

            return produtosNaoAmados
                .GroupBy(p => new { p.ProdutoSapId })
                .Select(p => new
                {
                    p.FirstOrDefault().ProdutoSapId,
                    p.FirstOrDefault().Descricao,
                    p.FirstOrDefault().Imagem,
                    Quantidade = p.Count()
                })
                .OrderByDescending(p => p.Quantidade)
                .Take(10)
                .ToList()
                .ConvertAll(p => new GaleriaTopDezProdutosNaoAmadosViewModel
                {
                    ProdutoSapId = p.ProdutoSapId,
                    Descricao = p.Descricao,
                    Imagem = p.Imagem,
                    Quantidade = p.Quantidade
                });
        }

        public async Task<int> ObterCartaoTarefasAtrasadas(UsuarioViewModel usuarioViewModel)
        {
            var tarefasAtrasadas = await _atendimentoApplication.ObterAtendimentoTarefasAtrasadas();

            if (usuarioViewModel.Tipo == UsuarioTipo.ADMINISTRADOR || usuarioViewModel.Tipo == UsuarioTipo.GERENTE)
            {
                return tarefasAtrasadas.Count();
            }
            else if (usuarioViewModel.Tipo == UsuarioTipo.VENDEDOR && usuarioViewModel.VendedorSapId != null)
            {
                return tarefasAtrasadas.Count(t => t.Atendimento.VendedorId == Convert.ToInt32(usuarioViewModel.VendedorSapId));
            }

            return 0;
        }
    }
}
