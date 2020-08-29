using Frattina.Application.ViewModels;
using Frattina.Business.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IAtendimentoApplication : IDisposable
    {
        Task<AtendimentoViewModel> Adicionar(AtendimentoViewModel atendimentoViewModel);

        Task Atualizar(AtendimentoViewModel atendimentoViewModel);

        Task AtualizarClienteVenda(AtendimentoViewModel atendimentoViewModel);

        Task Encerrar(AtendimentoViewModel atendimentoViewModel);

        Task Vender(AtendimentoViewModel atendimentoViewModel);

        Task Remover(Guid id);

        Task<AtendimentoProdutoViewModel> AdicionarProduto(AtendimentoProdutoViewModel atendimentoProdutoViewModel);

        Task AtualizarProdutoValorNegociado(AtendimentoProdutoViewModel atendimentoProdutoViewModel);

        Task AtualizarProdutoNivelInteresse(AtendimentoProdutoViewModel atendimentoProdutoViewModel);

        Task RemoverProdutoAtendimento(AtendimentoProdutoViewModel atendimentoProdutoViewModel);

        Task RemoverProdutoVenda(AtendimentoProdutoViewModel atendimentoProdutoViewModel);

        Task AdicionarProdutoVenda(AtendimentoProdutoViewModel atendimentoProdutoViewModel);

        Task<AtendimentoTarefaViewModel> AdicionarTarefa(AtendimentoTarefaViewModel atendimentoTarefaViewModel);

        Task AtualizarTarefa(AtendimentoTarefaViewModel atendimentoTarefaViewModel);

        Task RemoverTarefa(AtendimentoTarefaViewModel atendimentoTarefaViewModel);

        Task FinalizarTarefa(AtendimentoTarefaViewModel atendimentoTarefaViewModel);

        Task<AtendimentoViewModel> ObterAtendimento(Guid id);

        Task<AtendimentoViewModel> ObterAtendimentoProdutosTarefas(Guid id);

        Task<AtendimentoViewModel> ObterAtendimentoProdutosTarefasAuditoria(Guid id);

        Task<List<AtendimentoViewModel>> ObterAtendimentosProdutosTarefas();

        Task<List<AtendimentoViewModel>> ObterAtendimentosProdutosTarefasPorEtapa(AtendimentoEtapa etapa);

        Task<List<AtendimentoViewModel>> ObterAtendimentosProdutosTarefasPorVendedor(int vendedorId);

        Task<List<AtendimentoViewModel>> ObterAtendimentosProdutosTarefasPorEmpresa(int empresaId);

        Task<List<AtendimentoViewModel>> ObterAtendimentosProdutosTarefasPorCliente(string clienteId);

        Task<List<(string Vendedor, int QtdAtendimento, int QtdVenda)>> ObterAtendimentosAgrupadosPorVendedores();

        Task<AtendimentoProdutoViewModel> ObterAtendimentoProduto(Guid id);

        Task<List<AtendimentoProdutoViewModel>> ObterAtendimentoProdutosPorAtendimento(Guid atendimentoId);

        Task<List<AtendimentoProdutoViewModel>> ObterAtendimentoProdutosPorProduto(string ProdutoId);

        Task<List<AtendimentoProdutoViewModel>> ObterAtendimentoProdutosAmadosPorCliente(string clienteId, bool removerDuplicados);

        Task<List<AtendimentoProdutoViewModel>> ObterAtendimentoProdutosAmados();

        Task<List<AtendimentoProdutoViewModel>> ObterAtendimentoProdutosNaoAmados();

        Task<AtendimentoTarefaViewModel> ObterAtendimentoTarefa(Guid id);

        Task<List<AtendimentoTarefaViewModel>> ObterAtendimentoTarefasPorAtendimento(Guid atendimentoId);

        Task<List<AtendimentoTarefaViewModel>> ObterAtendimentoTarefasPorVendedor(int vendedorId);

        Task<List<AtendimentoTarefaViewModel>> ObterAtendimentoTarefasAtivas();

        Task<List<AtendimentoTarefaViewModel>> ObterAtendimentoTarefasAtrasadas();
    }
}
