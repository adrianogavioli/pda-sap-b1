using Frattina.Business.Models;
using System;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Repositories
{
    public interface IAtendimentoEncerradoRepository : IRepository<AtendimentoEncerrado>
    {
        Task<AtendimentoEncerrado> ObterAtendimentoEncerrado(Guid id);
    }
}
