using Frattina.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Business.Interfaces.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task Remover(Usuario usuario);

        Task<Usuario> ObterUsuario(Guid id);

        Task<Usuario> ObterUsuarioAuditoria(Guid id);

        Task<Usuario> ObterUsuarioPorUsuarioSap(int usuarioSapId);

        Task<Usuario> ObterUsuarioPorVendedorSap(int vendedorSapId);

        Task<IEnumerable<Usuario>> ObterUsuarios();
    }
}
