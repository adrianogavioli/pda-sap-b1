using Frattina.Business.Models;
using System;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Services
{
    public interface IAtendimentoService : IDisposable
    {
        Task Adicionar(Atendimento atendimento);

        Task Atualizar(Atendimento atendimento);

        Task AtualizarClienteVenda(Atendimento atendimento);

        Task Encerrar(Atendimento atendimento);

        Task Vender(Atendimento atendimento);

        Task Remover(Guid id);
    }
}
