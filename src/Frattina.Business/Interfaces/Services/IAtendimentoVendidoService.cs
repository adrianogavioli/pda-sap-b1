using Frattina.Business.Models;
using System;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Services
{
    public interface IAtendimentoVendidoService : IDisposable
    {
        Task Adicionar(AtendimentoVendido atendimentoVendido);
    }
}
