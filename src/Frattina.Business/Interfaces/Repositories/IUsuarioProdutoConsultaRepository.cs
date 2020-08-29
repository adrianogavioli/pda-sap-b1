using Frattina.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Repositories
{
    public interface IUsuarioProdutoConsultaRepository : IRepository<UsuarioProdutoConsulta>
    {
        Task<IEnumerable<UsuarioProdutoConsulta>> ObterUltimasVinteConsultasPorUsuario(Guid usuarioId);
    }
}
