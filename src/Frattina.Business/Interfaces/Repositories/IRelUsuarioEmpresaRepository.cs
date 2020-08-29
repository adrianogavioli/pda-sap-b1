using Frattina.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Repositories
{
    public interface IRelUsuarioEmpresaRepository : IRepository<RelUsuarioEmpresa>
    {
        Task<RelUsuarioEmpresa> ObterRelUsuarioEmpresa(Guid id);

        Task<IEnumerable<RelUsuarioEmpresa>> ObterRelUsuarioEmpresaPorUsuario(Guid usuarioId);

        Task<IEnumerable<RelUsuarioEmpresa>> ObterRelUsuarioEmpresaPorEmpresa(int empresaId);
    }
}
