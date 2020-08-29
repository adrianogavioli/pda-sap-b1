using AutoMapper;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Interfaces.Services;
using Frattina.Business.Models;
using Frattina.Business.Models.Enums;
using Frattina.CrossCutting.Matematica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.Application.Services
{
    public class AtendimentoApplication : BaseApplication, IAtendimentoApplication
    {
        private readonly IAtendimentoService _atendimentoService;
        private readonly IAtendimentoRepository _atendimentoRepository;
        private readonly IAtendimentoProdutoService _atendimentoProdutoService;
        private readonly IAtendimentoProdutoRepository _atendimentoProdutoRepository;
        private readonly IAtendimentoTarefaService _atendimentoTarefaService;
        private readonly IAtendimentoTarefaRepository _atendimentoTarefaRepository;
        private readonly IEmpresaApplication _empresaApplication;
        private readonly IClienteApplication _clienteApplication;
        private readonly IClienteAtendimentoApplication _clienteAtendimentoApplication;
        private readonly IVendedorApplication _vendedorApplication;
        private readonly IProdutoApplication _produtoApplication;
        private readonly IMapper _mapper;

        public AtendimentoApplication(
            IAtendimentoService atendimentoService,
            IAtendimentoRepository atendimentoRepository,
            IAtendimentoProdutoService atendimentoProdutoService,
            IAtendimentoProdutoRepository atendimentoProdutoRepository,
            IAtendimentoTarefaService atendimentoTarefaService,
            IAtendimentoTarefaRepository atendimentoTarefaRepository,
            IEmpresaApplication empresaApplication,
            IClienteApplication clienteApplication,
            IClienteAtendimentoApplication clienteAtendimentoApplication,
            IVendedorApplication vendedorApplication,
            IProdutoApplication produtoApplication,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _atendimentoService = atendimentoService;
            _atendimentoRepository = atendimentoRepository;
            _atendimentoProdutoService = atendimentoProdutoService;
            _atendimentoProdutoRepository = atendimentoProdutoRepository;
            _atendimentoTarefaService = atendimentoTarefaService;
            _atendimentoTarefaRepository = atendimentoTarefaRepository;
            _empresaApplication = empresaApplication;
            _clienteApplication = clienteApplication;
            _clienteAtendimentoApplication = clienteAtendimentoApplication;
            _vendedorApplication = vendedorApplication;
            _produtoApplication = produtoApplication;
            _mapper = mapper;
        }

        public async Task<AtendimentoViewModel> Adicionar(AtendimentoViewModel atendimentoViewModel)
        {
            await PreencherEmpresa(atendimentoViewModel);

            if (string.IsNullOrEmpty(atendimentoViewModel.EmpresaNome)) return null;

            await PreencherVendedor(atendimentoViewModel);

            if (string.IsNullOrEmpty(atendimentoViewModel.VendedorNome)) return null;

            await _clienteAtendimentoApplication.GerenciarClienteAtendimento(atendimentoViewModel);

            if (!OperacaoValida()) return null;

            var atendimento = _mapper.Map<Atendimento>(atendimentoViewModel);

            await _atendimentoService.Adicionar(atendimento);

            if (!OperacaoValida()) return null;

            atendimentoViewModel = await ObterAtendimentoProdutosTarefas(atendimento.Id);

            if (atendimentoViewModel == null)
            {
                Notificar("Não foi possível adicionar o atendimento.");

                return null;
            }

            return atendimentoViewModel;
        }

        public async Task Atualizar(AtendimentoViewModel atendimentoViewModel)
        {
            await PreencherVendedor(atendimentoViewModel);

            if (string.IsNullOrEmpty(atendimentoViewModel.VendedorNome)) return;

            var cliente = await _clienteApplication.ObterCliente(atendimentoViewModel.ClienteId);

            if (cliente == null)
            {
                Notificar("Não foi possível obter as informações do cliente.");

                return;
            }

            atendimentoViewModel.ClienteNome = cliente.Nome;

            await _atendimentoService.Atualizar(_mapper.Map<Atendimento>(atendimentoViewModel));
        }

        public async Task AtualizarClienteVenda(AtendimentoViewModel atendimentoViewModel)
        {
            var cliente = _clienteApplication.ObterPorNome(atendimentoViewModel.ClienteNomeVenda).Result.FirstOrDefault();
            if (cliente == null)
            {
                Notificar("Não foi possível obter os dados do cliente, certifique-se que o nome esteja correto.");
                return;
            }

            atendimentoViewModel.ClienteIdVenda = cliente.Id;

            await _atendimentoService.AtualizarClienteVenda(_mapper.Map<Atendimento>(atendimentoViewModel));
        }

        public async Task Encerrar(AtendimentoViewModel atendimentoViewModel)
        {
            if (atendimentoViewModel.Encerrado == null)
            {
                Notificar("É necessário informar os dados do encerramento.");
                return;
            }

            await _atendimentoService.Encerrar(_mapper.Map<Atendimento>(atendimentoViewModel));
        }

        public async Task Vender(AtendimentoViewModel atendimentoViewModel)
        {
            if (atendimentoViewModel.Vendido == null)
            {
                Notificar("É necessário informar os dados da venda.");
                return;
            }

            await _atendimentoService.Vender(_mapper.Map<Atendimento>(atendimentoViewModel));
        }

        public async Task Remover(Guid id)
        {
            await _atendimentoService.Remover(id);
        }

        public async Task<AtendimentoProdutoViewModel> AdicionarProduto(AtendimentoProdutoViewModel atendimentoProdutoViewModel)
        {
            var produto = await _produtoApplication.ObterProduto(atendimentoProdutoViewModel.ProdutoSapId);

            if (produto == null)
            {
                Notificar("O produto é inválido.");
                return null;
            }

            var atendimentoProduto = _mapper.Map<AtendimentoProduto>(atendimentoProdutoViewModel);

            atendimentoProduto.ProdutoSapId = produto.Id;
            atendimentoProduto.Tipo = produto.Tipo.Nome;
            atendimentoProduto.Marca = produto.Marca.Nome;
            atendimentoProduto.Modelo = produto.Modelo.Nome;
            atendimentoProduto.Referencia = produto.Referencia;
            atendimentoProduto.Descricao = produto.Descricao;
            atendimentoProduto.Imagem = produto.Imagem;
            atendimentoProduto.ValorTabela = produto.Precos.Max(p => p.Valor);

            await _atendimentoProdutoService.Adicionar(atendimentoProduto);

            if (!OperacaoValida()) return null;

            atendimentoProdutoViewModel = await ObterAtendimentoProduto(atendimentoProduto.Id);

            if (atendimentoProdutoViewModel == null)
            {
                Notificar("Não foi possível adicionar o produto ao atendimento.");
                return null;
            }

            return atendimentoProdutoViewModel;
        }

        public async Task AtualizarProdutoValorNegociado(AtendimentoProdutoViewModel atendimentoProdutoViewModel)
        {
            await _atendimentoProdutoService.AtualizarValorNegociado(_mapper.Map<AtendimentoProduto>(atendimentoProdutoViewModel));
        }

        public async Task AtualizarProdutoNivelInteresse(AtendimentoProdutoViewModel atendimentoProdutoViewModel)
        {
            await _atendimentoProdutoService.AtualizarNivelInteresse(_mapper.Map<AtendimentoProduto>(atendimentoProdutoViewModel));
        }

        public async Task RemoverProdutoAtendimento(AtendimentoProdutoViewModel atendimentoProdutoViewModel)
        {
            await _atendimentoProdutoService.RemoverDoAtendimento(_mapper.Map<AtendimentoProduto>(atendimentoProdutoViewModel));
        }

        public async Task RemoverProdutoVenda(AtendimentoProdutoViewModel atendimentoProdutoViewModel)
        {
            await _atendimentoProdutoService.RemoverDaVenda(_mapper.Map<AtendimentoProduto>(atendimentoProdutoViewModel));
        }

        public async Task AdicionarProdutoVenda(AtendimentoProdutoViewModel atendimentoProdutoViewModel)
        {
            await _atendimentoProdutoService.AdicionarAVenda(_mapper.Map<AtendimentoProduto>(atendimentoProdutoViewModel));
        }

        public async Task<AtendimentoTarefaViewModel> AdicionarTarefa(AtendimentoTarefaViewModel atendimentoTarefaViewModel)
        {
            var atendimentoTarefa = _mapper.Map<AtendimentoTarefa>(atendimentoTarefaViewModel);

            await _atendimentoTarefaService.Adicionar(atendimentoTarefa);

            if (!OperacaoValida()) return null;

            atendimentoTarefaViewModel = await ObterAtendimentoTarefa(atendimentoTarefa.Id);

            if (atendimentoTarefaViewModel == null)
            {
                Notificar("Não foi possível adicionar a tarefa ao atendimento.");
                return null;
            }

            return atendimentoTarefaViewModel;
        }

        public async Task AtualizarTarefa(AtendimentoTarefaViewModel atendimentoTarefaViewModel)
        {
            await _atendimentoTarefaService.Atualizar(_mapper.Map<AtendimentoTarefa>(atendimentoTarefaViewModel));
        }

        public async Task RemoverTarefa(AtendimentoTarefaViewModel atendimentoTarefaViewModel)
        {
            await _atendimentoTarefaService.Remover(_mapper.Map<AtendimentoTarefa>(atendimentoTarefaViewModel));
        }

        public async Task FinalizarTarefa(AtendimentoTarefaViewModel atendimentoTarefaViewModel)
        {
            await _atendimentoTarefaService.Finalizar(_mapper.Map<AtendimentoTarefa>(atendimentoTarefaViewModel));
        }

        public async Task<AtendimentoViewModel> ObterAtendimento(Guid id)
        {
            var atendimentoViewModel = _mapper.Map<AtendimentoViewModel>(await _atendimentoRepository.ObterAtendimento(id));

            if (atendimentoViewModel == null) return null;

            return atendimentoViewModel;
        }

        public async Task<AtendimentoViewModel> ObterAtendimentoProdutosTarefas(Guid id)
        {
            var atendimentoViewModel = _mapper.Map<AtendimentoViewModel>(await _atendimentoRepository.ObterAtendimentoProdutosTarefas(id));

            if (atendimentoViewModel == null) return null;

            await CalcularValoresTotaisAtendimento(atendimentoViewModel);

            await CalcularPercentsDescontosProdutos(atendimentoViewModel.Produtos);

            atendimentoViewModel.PercentTotalDesconto = Calculos.CalcularDesconto(atendimentoViewModel.ValorTotalTabela, atendimentoViewModel.ValorTotalNegociado);

            return atendimentoViewModel;
        }

        public async Task<AtendimentoViewModel> ObterAtendimentoProdutosTarefasAuditoria(Guid id)
        {
            return _mapper.Map<AtendimentoViewModel>(await _atendimentoRepository.ObterAtendimentoProdutosTarefasAuditoria(id));
        }

        public async Task<List<AtendimentoViewModel>> ObterAtendimentosProdutosTarefas()
        {
            var atendimentosViewModel = _mapper.Map<List<AtendimentoViewModel>>(await _atendimentoRepository.ObterAtendimentosProdutosTarefas());

            foreach (var atendimentoViewModel in atendimentosViewModel)
            {
                await CalcularValoresTotaisAtendimento(atendimentoViewModel);

                await CalcularPercentsDescontosProdutos(atendimentoViewModel.Produtos);

                atendimentoViewModel.PercentTotalDesconto = Calculos.CalcularDesconto(atendimentoViewModel.ValorTotalTabela, atendimentoViewModel.ValorTotalNegociado);
            }

            return atendimentosViewModel;
        }

        public async Task<List<AtendimentoViewModel>> ObterAtendimentosProdutosTarefasPorCliente(string clienteId)
        {
            var atendimentosViewModel = _mapper.Map<List<AtendimentoViewModel>>(await _atendimentoRepository.ObterAtendimentosProdutosTarefasPorCliente(clienteId));

            foreach (var atendimentoViewModel in atendimentosViewModel)
            {
                await CalcularValoresTotaisAtendimento(atendimentoViewModel);

                await CalcularPercentsDescontosProdutos(atendimentoViewModel.Produtos);

                atendimentoViewModel.PercentTotalDesconto = Calculos.CalcularDesconto(atendimentoViewModel.ValorTotalTabela, atendimentoViewModel.ValorTotalNegociado);
            }

            return atendimentosViewModel;
        }

        public async Task<List<AtendimentoViewModel>> ObterAtendimentosProdutosTarefasPorEmpresa(int empresaId)
        {
            var atendimentosViewModel = _mapper.Map<List<AtendimentoViewModel>>(await _atendimentoRepository.ObterAtendimentosProdutosTarefasPorEmpresa(empresaId));

            foreach (var atendimentoViewModel in atendimentosViewModel)
            {
                await CalcularValoresTotaisAtendimento(atendimentoViewModel);

                await CalcularPercentsDescontosProdutos(atendimentoViewModel.Produtos);

                atendimentoViewModel.PercentTotalDesconto = Calculos.CalcularDesconto(atendimentoViewModel.ValorTotalTabela, atendimentoViewModel.ValorTotalNegociado);
            }

            return atendimentosViewModel;
        }

        public async Task<List<AtendimentoViewModel>> ObterAtendimentosProdutosTarefasPorEtapa(AtendimentoEtapa etapa)
        {
            var atendimentosViewModel = _mapper.Map<List<AtendimentoViewModel>>(await _atendimentoRepository.ObterAtendimentosProdutosTarefasPorEtapa(etapa));

            foreach (var atendimentoViewModel in atendimentosViewModel)
            {
                await CalcularValoresTotaisAtendimento(atendimentoViewModel);

                await CalcularPercentsDescontosProdutos(atendimentoViewModel.Produtos);

                atendimentoViewModel.PercentTotalDesconto = Calculos.CalcularDesconto(atendimentoViewModel.ValorTotalTabela, atendimentoViewModel.ValorTotalNegociado);
            }

            return atendimentosViewModel;
        }

        public async Task<List<AtendimentoViewModel>> ObterAtendimentosProdutosTarefasPorVendedor(int vendedorId)
        {
            var atendimentosViewModel = _mapper.Map<List<AtendimentoViewModel>>(await _atendimentoRepository.ObterAtendimentosProdutosTarefasPorVendedor(vendedorId));

            foreach (var atendimentoViewModel in atendimentosViewModel)
            {
                await CalcularValoresTotaisAtendimento(atendimentoViewModel);

                await CalcularPercentsDescontosProdutos(atendimentoViewModel.Produtos);

                atendimentoViewModel.PercentTotalDesconto = Calculos.CalcularDesconto(atendimentoViewModel.ValorTotalTabela, atendimentoViewModel.ValorTotalNegociado);
            }

            return atendimentosViewModel;
        }

        public async Task<List<(string Vendedor, int QtdAtendimento, int QtdVenda)>> ObterAtendimentosAgrupadosPorVendedores()
        {
            var atendimentos = await _atendimentoRepository.ObterAtendimentosAgrupadosPorVendedores();

            return atendimentos.ToList();
        }

        public async Task<AtendimentoProdutoViewModel> ObterAtendimentoProduto(Guid id)
        {
            var atendimentoProdutoViewModel = _mapper.Map<AtendimentoProdutoViewModel>(await _atendimentoProdutoRepository.ObterAtendimentoProduto(id));

            if (atendimentoProdutoViewModel == null) return null;

            atendimentoProdutoViewModel.PercentDesconto = Calculos.CalcularDesconto((decimal)atendimentoProdutoViewModel.ValorTabela, (decimal)atendimentoProdutoViewModel.ValorNegociado);

            return atendimentoProdutoViewModel;
        }

        public async Task<List<AtendimentoProdutoViewModel>> ObterAtendimentoProdutosPorAtendimento(Guid atendimentoId)
        {
            var atendimentoProdutosViewModel = _mapper.Map<List<AtendimentoProdutoViewModel>>(await _atendimentoProdutoRepository.ObterAtendimentoProdutosPorAtendimento(atendimentoId));

            await CalcularPercentsDescontosProdutos(atendimentoProdutosViewModel);

            return atendimentoProdutosViewModel;
        }

        public async Task<List<AtendimentoProdutoViewModel>> ObterAtendimentoProdutosPorProduto(string ProdutoId)
        {
            return _mapper.Map<List<AtendimentoProdutoViewModel>>(await _atendimentoProdutoRepository.ObterAtendimentoProdutosPorProduto(ProdutoId));
        }

        public async Task<List<AtendimentoProdutoViewModel>> ObterAtendimentoProdutosAmadosPorCliente(string clienteId, bool removerDuplicados)
        {
            return await _clienteAtendimentoApplication.ObterAtendimentoProdutosAmadosPorCliente(clienteId, removerDuplicados);
        }

        public async Task<List<AtendimentoProdutoViewModel>> ObterAtendimentoProdutosAmados()
        {
            return _mapper.Map<List<AtendimentoProdutoViewModel>>(await _atendimentoProdutoRepository.ObterAtendimentoProdutosAmados());
        }

        public async Task<List<AtendimentoProdutoViewModel>> ObterAtendimentoProdutosNaoAmados()
        {
            return _mapper.Map<List<AtendimentoProdutoViewModel>>(await _atendimentoProdutoRepository.ObterAtendimentoProdutosNaoAmados());
        }

        public async Task<AtendimentoTarefaViewModel> ObterAtendimentoTarefa(Guid id)
        {
            return _mapper.Map<AtendimentoTarefaViewModel>(await _atendimentoTarefaRepository.ObterAtendimentoTarefa(id));
        }

        public async Task<List<AtendimentoTarefaViewModel>> ObterAtendimentoTarefasPorAtendimento(Guid atendimentoId)
        {
            return _mapper.Map<List<AtendimentoTarefaViewModel>>(await _atendimentoTarefaRepository.ObterAtendimentoTarefasPorAtendimento(atendimentoId));
        }

        public async Task<List<AtendimentoTarefaViewModel>> ObterAtendimentoTarefasPorVendedor(int vendedorId)
        {
            return _mapper.Map<List<AtendimentoTarefaViewModel>>(await _atendimentoTarefaRepository.ObterAtendimentoTarefasPorVendedor(vendedorId));
        }

        public async Task<List<AtendimentoTarefaViewModel>> ObterAtendimentoTarefasAtivas()
        {
            return _mapper.Map<List<AtendimentoTarefaViewModel>>(await _atendimentoTarefaRepository.ObterAtendimentoTarefasAtivas());
        }

        public async Task<List<AtendimentoTarefaViewModel>> ObterAtendimentoTarefasAtrasadas()
        {
            return _mapper.Map<List<AtendimentoTarefaViewModel>>(await _atendimentoTarefaRepository.ObterAtendimentoTarefasAtrasadas());
        }

        private Task<AtendimentoViewModel> CalcularValoresTotaisAtendimento(AtendimentoViewModel atendimentoViewModel)
        {
            if (atendimentoViewModel.Produtos != null)
            {
                foreach (var item in atendimentoViewModel.Produtos)
                {
                    atendimentoViewModel.ValorTotalTabela += Convert.ToDecimal(item.ValorTabela);
                    atendimentoViewModel.ValorTotalNegociado += Convert.ToDecimal(item.ValorNegociado);
                }
            }

            return Task.FromResult(atendimentoViewModel);
        }

        private Task<List<AtendimentoProdutoViewModel>> CalcularPercentsDescontosProdutos(List<AtendimentoProdutoViewModel> atendimentoProdutosViewModel)
        {
            foreach (var atendimentoProdutoViewModel in atendimentoProdutosViewModel)
            {
                atendimentoProdutoViewModel.PercentDesconto = Calculos.CalcularDesconto((decimal)atendimentoProdutoViewModel.ValorTabela, (decimal)atendimentoProdutoViewModel.ValorNegociado);
            }

            return Task.FromResult(atendimentoProdutosViewModel);
        }

        private async Task<AtendimentoViewModel> PreencherEmpresa(AtendimentoViewModel atendimentoViewModel)
        {
            var empresa = await _empresaApplication.ObterEmpresa(atendimentoViewModel.EmpresaId);

            if (empresa == null)
            {
                Notificar("Não foi possível obter as informações da empresa.");

                return null;
            }

            atendimentoViewModel.EmpresaNome = empresa.RazaoSocial;

            return atendimentoViewModel;
        }

        private async Task<AtendimentoViewModel> PreencherVendedor(AtendimentoViewModel atendimentoViewModel)
        {
            var vendedor = await _vendedorApplication.ObterVendedor(atendimentoViewModel.VendedorId);

            if (vendedor == null)
            {
                Notificar("Não foi possível obter as informações do vendedor.");

                return null;
            }

            atendimentoViewModel.VendedorNome = vendedor.Nome;

            return atendimentoViewModel;
        }

        public void Dispose()
        {
            _atendimentoService?.Dispose();
            _atendimentoRepository?.Dispose();
            _atendimentoProdutoService?.Dispose();
            _atendimentoProdutoRepository?.Dispose();
            _atendimentoTarefaService?.Dispose();
            _atendimentoTarefaRepository?.Dispose();
            _empresaApplication?.Dispose();
            _clienteApplication?.Dispose();
            _clienteAtendimentoApplication?.Dispose();
            _vendedorApplication?.Dispose();
            _produtoApplication?.Dispose();
        }
    }
}
