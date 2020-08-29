using Frattina.Business.Models;
using System;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Repositories
{
    public interface IAtendimentoVendidoRepository : IRepository<AtendimentoVendido>
    {
        Task<AtendimentoVendido> ObterAtendimentoVendido(Guid id);
    }
}
