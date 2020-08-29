using Frattina.Business.Interfaces;
using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Interfaces.Services;
using Frattina.Business.Models;
using Frattina.Business.Models.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.Business.Services
{
    public class UsuarioService : BaseService, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IRelUsuarioEmpresaRepository _relUsuarioEmpresaRepository;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IRelUsuarioEmpresaRepository relUsuarioEmpresaRepository,
            INotificador notificador) : base(notificador)
        {
            _usuarioRepository = usuarioRepository;
            _relUsuarioEmpresaRepository = relUsuarioEmpresaRepository;
        }

        public async Task Adicionar(Usuario usuario)
        {
            if (!ExecutarValidacao(new UsuarioValidation(), usuario)) return;

            if (usuario.UsuarioSapId != null && _usuarioRepository.ObterUsuarioPorUsuarioSap(Convert.ToInt32(usuario.UsuarioSapId)).Result != null)
            {
                Notificar("O Usuário SAP informado já está vinculado a outro usuário.");
                return;
            }

            if (usuario.VendedorSapId != null && _usuarioRepository.ObterUsuarioPorVendedorSap(Convert.ToInt32(usuario.VendedorSapId)).Result != null)
            {
                Notificar("O Vendedor SAP informado já está vinculado a outro usuário.");
                return;
            }

            usuario.Cargo = null;

            await _usuarioRepository.Adicionar(usuario);
        }

        public async Task Atualizar(Usuario usuario)
        {
            var usuarioDb = await _usuarioRepository.ObterPorId(usuario.Id);

            if (usuarioDb == null)
            {
                Notificar("Não foi possível obter as informações do usuário.");
                return;
            }

            if (!ExecutarValidacao(new UsuarioValidation(), usuario)) return;

            if (usuario.UsuarioSapId != null && _usuarioRepository.Buscar(u => u.UsuarioSapId == usuario.UsuarioSapId && u.Id != usuario.Id).Result.Any())
            {
                Notificar("O Usuário SAP informado já está vinculado a outro usuário.");
                return;
            }

            if (usuario.VendedorSapId != null && _usuarioRepository.Buscar(u => u.VendedorSapId == usuario.VendedorSapId && u.Id != usuario.Id).Result.Any())
            {
                Notificar("O Vendedor SAP informado já está vinculado a outro usuário.");
                return;
            }

            usuarioDb.Tipo = usuario.Tipo;
            usuarioDb.Nome = usuario.Nome;
            usuarioDb.CargoId = usuario.CargoId;
            usuarioDb.UsuarioSapId = usuario.UsuarioSapId;
            usuarioDb.UsuarioSapNome = usuario.UsuarioSapNome;
            usuarioDb.VendedorSapId = usuario.VendedorSapId;
            usuarioDb.VendedorSapNome = usuario.VendedorSapNome;

            await _usuarioRepository.Atualizar(usuarioDb);
        }

        public async Task Remover(Usuario usuario)
        {
            await _usuarioRepository.Remover(usuario);
        }

        public void Dispose()
        {
            _usuarioRepository?.Dispose();
        }
    }
}
