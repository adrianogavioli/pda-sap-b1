using Frattina.Business.Models;
using System;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Services
{
    public interface IAtendimentoEncerradoService : IDisposable
    {
        Task Adicionar(AtendimentoEncerrado atendimentoEncerrado);
    }
}
