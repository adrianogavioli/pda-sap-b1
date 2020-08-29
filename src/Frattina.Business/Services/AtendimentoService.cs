using Frattina.Business.Interfaces;
using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Interfaces.Services;
using Frattina.Business.Models;
using Frattina.Business.Models.Enums;
using Frattina.Business.Models.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.Business.Services
{
    public class AtendimentoService : BaseService, IAtendimentoService
    {
        private readonly IAtendimentoRepository _atendimentoRepository;
        private readonly IAtendimentoEncerradoService _atendimentoEncerradoService;
        private readonly IAtendimentoVendidoService _atendimentoVendidoService;

        public AtendimentoService(IAtendimentoRepository atendimentoRepository,
            IAtendimentoEncerradoService atendimentoEncerradoService,
            IAtendimentoVendidoService atendimentoVendidoService,
            INotificador notificador) : base(notificador)
        {
            _atendimentoRepository = atendimentoRepository;
            _atendimentoEncerradoService = atendimentoEncerradoService;
            _atendimentoVendidoService = atendimentoVendidoService;
        }

        public async Task Adicionar(Atendimento atendimento)
        {
            atendimento.Data = DateTime.Now;
            atendimento.Etapa = AtendimentoEtapa.Andamento;
            atendimento.ClienteIdVenda = atendimento.ClienteId;
            atendimento.ClienteNomeVenda = atendimento.ClienteNome;
            
            if (!ExecutarValidacao(new AtendimentoValidation(), atendimento)) return;

            await _atendimentoRepository.Adicionar(atendimento);
        }

        public async Task Atualizar(Atendimento atendimento)
        {
            var atendimentoDb = await _atendimentoRepository.ObterPorId(atendimento.Id);

            if (atendimentoDb == null)
            {
                Notificar("Não foi possível obter as informações do atendimento.");
                return;
            }

            if (atendimentoDb.Etapa != AtendimentoEtapa.Andamento)
            {
                Notificar("Somente atendimentos em andamentos podem ser atualizados.");
                return;
            }

            atendimentoDb.VendedorId = atendimento.VendedorId;
            atendimentoDb.ClienteNome = atendimento.ClienteNome;
            atendimentoDb.ClienteNomeVenda = atendimento.ClienteNome;
            atendimentoDb.Negociacao = atendimento.Negociacao;

            if (!ExecutarValidacao(new AtendimentoValidation(), atendimentoDb)) return;

            await _atendimentoRepository.Atualizar(atendimentoDb);
        }

        public async Task AtualizarClienteVenda(Atendimento atendimento)
        {
            var atendimentoDb = await _atendimentoRepository.ObterPorId(atendimento.Id);

            if (atendimentoDb == null)
            {
                Notificar("Não foi possível obter as informações do atendimento.");
                return;
            }

            if (atendimentoDb.Etapa != AtendimentoEtapa.Andamento)
            {
                Notificar("Somente atendimentos em andamentos podem ser atualizados.");
                return;
            }

            atendimentoDb.ClienteIdVenda = atendimento.ClienteIdVenda;
            atendimentoDb.ClienteNomeVenda = atendimento.ClienteNomeVenda;

            if (!ExecutarValidacao(new AtendimentoValidation(), atendimentoDb)) return;

            await _atendimentoRepository.Atualizar(atendimentoDb);
        }

        public async Task Encerrar(Atendimento atendimento)
        {
            var atendimentoDb = await _atendimentoRepository.ObterPorId(atendimento.Id);

            if (atendimentoDb == null)
            {
                Notificar("Não foi possível obter as informações do atendimento.");
                return;
            }

            if (atendimentoDb.Etapa != AtendimentoEtapa.Andamento)
            {
                Notificar("Somente atendimentos em andamentos podem ser encerrados.");
                return;
            }

            if (atendimento.Encerrado == null)
            {
                Notificar("É necessário informar os dados do encerramento.");
                return;
            }

            atendimento.Encerrado.AtendimentoId = atendimento.Id;

            await _atendimentoEncerradoService.Adicionar(atendimento.Encerrado);

            atendimentoDb.Etapa = AtendimentoEtapa.Encerrado;

            await _atendimentoRepository.Atualizar(atendimentoDb);
        }

        public async Task Vender(Atendimento atendimento)
        {
            var atendimentoDb = await _atendimentoRepository.ObterPorId(atendimento.Id);

            if (atendimentoDb == null)
            {
                Notificar("Não foi possível obter as informações do atendimento.");
                return;
            }

            if (atendimentoDb.Etapa != AtendimentoEtapa.Andamento)
            {
                Notificar("Somente atendimentos em andamentos podem ser vendidos.");
                return;
            }

            if (atendimento.Vendido == null)
            {
                Notificar("É necessário informar os dados da venda.");
                return;
            }

            atendimento.Vendido.AtendimentoId = atendimento.Id;

            await _atendimentoVendidoService.Adicionar(atendimento.Vendido);

            atendimentoDb.Etapa = AtendimentoEtapa.Vendido;

            await _atendimentoRepository.Atualizar(atendimentoDb);
        }

        public async Task Remover(Guid id)
        {
            var atendimentoDb = await _atendimentoRepository.ObterAtendimentoProdutosTarefas(id);

            if (atendimentoDb == null)
            {
                Notificar("Não foi possível obter as informações do atendimento.");
                return;
            }

            if (atendimentoDb.Etapa != AtendimentoEtapa.Andamento)
            {
                Notificar("Somente atendimentos em andamentos podem ser removidos.");
                return;
            }

            if (atendimentoDb.Produtos.Any())
            {
                Notificar("Uma vez adicionado produto ao atendimento, não é possível removê-lo.");
                return;
            }

            if (atendimentoDb.Tarefas.Any())
            {
                Notificar("Uma vez adicionada tarefa ao atendimento, não é possível removê-lo.");
                return;
            }

            await _atendimentoRepository.Remover(id);
        }

        public void Dispose()
        {
            _atendimentoRepository?.Dispose();
            _atendimentoEncerradoService?.Dispose();
            _atendimentoVendidoService?.Dispose();
        }
    }
}
