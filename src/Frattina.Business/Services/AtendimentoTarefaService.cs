using Frattina.Business.Interfaces;
using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Interfaces.Services;
using Frattina.Business.Models;
using Frattina.Business.Models.Validations;
using System;
using System.Threading.Tasks;

namespace Frattina.Business.Services
{
    public class AtendimentoTarefaService : BaseService, IAtendimentoTarefaService
    {
        private readonly IAtendimentoTarefaRepository _atendimentoTarefaRepository;

        public AtendimentoTarefaService(IAtendimentoTarefaRepository atendimentoTarefaRepository,
            INotificador notificador) : base(notificador)
        {
            _atendimentoTarefaRepository = atendimentoTarefaRepository;
        }

        public async Task Adicionar(AtendimentoTarefa atendimentoTarefa)
        {
            atendimentoTarefa.Removida = false;

            if (!ExecutarValidacao(new AtendimentoTarefaValidation(), atendimentoTarefa)) return;

            await _atendimentoTarefaRepository.Adicionar(atendimentoTarefa);
        }

        public async Task Atualizar(AtendimentoTarefa atendimentoTarefa)
        {
            var atendimentoTarefaDb = await _atendimentoTarefaRepository.ObterPorId(atendimentoTarefa.Id);

            if (atendimentoTarefaDb == null)
            {
                Notificar("Não foi possível obter as informações da tarefa.");
                return;
            }

            if (atendimentoTarefaDb.DataFinalizacao != null)
            {
                Notificar("Tarefas finalizadas não podem ser alteradas.");
                return;
            }

            atendimentoTarefaDb.Tipo = atendimentoTarefa.Tipo;
            atendimentoTarefaDb.Assunto = atendimentoTarefa.Assunto;
            atendimentoTarefaDb.DataTarefa = atendimentoTarefa.DataTarefa;

            if (!ExecutarValidacao(new AtendimentoTarefaValidation(), atendimentoTarefaDb)) return;

            await _atendimentoTarefaRepository.Atualizar(atendimentoTarefaDb);
        }

        public async Task Remover(AtendimentoTarefa atendimentoTarefa)
        {
            var atendimentoTarefaDb = await _atendimentoTarefaRepository.ObterPorId(atendimentoTarefa.Id);

            if (atendimentoTarefaDb == null)
            {
                Notificar("Não foi possível obter as informações da tarefa.");
                return;
            }

            if (atendimentoTarefaDb.DataFinalizacao != null)
            {
                Notificar("Tarefas finalizadas não podem ser removidas.");
                return;
            }

            atendimentoTarefaDb.Removida = true;

            await _atendimentoTarefaRepository.Atualizar(atendimentoTarefaDb);
        }

        public async Task Finalizar(AtendimentoTarefa atendimentoTarefa)
        {
            var atendimentoTarefaDb = await _atendimentoTarefaRepository.ObterPorId(atendimentoTarefa.Id);

            if (atendimentoTarefaDb == null)
            {
                Notificar("Não foi possível obter as informações da tarefa.");
                return;
            }

            atendimentoTarefaDb.DataFinalizacao = DateTime.Now;

            await _atendimentoTarefaRepository.Atualizar(atendimentoTarefaDb);
        }

        public void Dispose()
        {
            _atendimentoTarefaRepository?.Dispose();
        }
    }
}
