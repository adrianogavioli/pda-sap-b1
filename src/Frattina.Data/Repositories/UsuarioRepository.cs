using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Models;
using Frattina.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.Data.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(FrattinaDbContext context) : base (context) { }

        public async Task Remover(Usuario usuario)
        {
            Db.Usuarios.Remove(usuario);
            await SaveChanges();
        }

        public async Task<Usuario> ObterUsuario(Guid id)
        {
            var usuario = await Db.Usuarios
                .AsNoTracking()
                .Include(u => u.Cargo)
                .Include(u => u.Empresas)
                .FirstOrDefaultAsync(u => u.Id == id);

            usuario.Empresas = usuario.Empresas.Where(e => !e.Removido);

            return usuario;
        }

        public async Task<Usuario> ObterUsuarioAuditoria(Guid id)
        {
            return await Db.Usuarios
                .AsNoTracking()
                .Include(u => u.Empresas)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario> ObterUsuarioPorUsuarioSap(int usuarioSapId)
        {
            return await Db.Usuarios
                .AsNoTracking()
                .Include(u => u.Cargo)
                .FirstOrDefaultAsync(u => u.UsuarioSapId == usuarioSapId);
        }

        public async Task<Usuario> ObterUsuarioPorVendedorSap(int vendedorSapId)
        {
            return await Db.Usuarios
                .AsNoTracking()
                .Include(u => u.Cargo)
                .FirstOrDefaultAsync(u => u.VendedorSapId == vendedorSapId);
        }

        public async Task<IEnumerable<Usuario>> ObterUsuarios()
        {
            return await Db.Usuarios
                .AsNoTracking()
                .Include(u => u.Cargo)
                .ToListAsync();
        }
    }
}
