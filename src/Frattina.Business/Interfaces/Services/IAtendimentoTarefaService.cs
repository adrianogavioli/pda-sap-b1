using Frattina.Business.Models;
using System;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Services
{
    public interface IAtendimentoTarefaService : IDisposable
    {
        Task Adicionar(AtendimentoTarefa atendimentoTarefa);

        Task Atualizar(AtendimentoTarefa atendimentoTarefa);

        Task Remover(AtendimentoTarefa atendimentoTarefa);

        Task Finalizar(AtendimentoTarefa atendimentoTarefa);
    }
}
