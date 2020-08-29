using AutoMapper;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.Business.Interfaces.Repositories;
using Frattina.Business.Interfaces.Services;
using Frattina.Business.Models;
using Frattina.CrossCutting.StringTools;
using Frattina.CrossCutting.UsuarioIdentity;
using Frattina.SapService.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Frattina.Application.Services
{
    public class UsuarioApplication : BaseApplication, IUsuarioApplication
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioSapService _usuarioSapService;
        private readonly IVendedorSapService _vendedorSapService;
        private readonly IUsuarioIdentityService _usuarioIdentityService;
        private readonly IMapper _mapper;

        public UsuarioApplication(
            IUsuarioService usuarioService,
            IUsuarioRepository usuarioRepository,
            IUsuarioSapService usuarioSapService,
            IVendedorSapService vendedorSapService,
            IUsuarioIdentityService usuarioIdentityService,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _usuarioService = usuarioService;
            _usuarioRepository = usuarioRepository;
            _usuarioSapService = usuarioSapService;
            _vendedorSapService = vendedorSapService;
            _usuarioIdentityService = usuarioIdentityService;
            _mapper = mapper;
        }

        public async Task<UsuarioViewModel> Adicionar(UsuarioViewModel usuarioViewModel)
        {
            await PreencherNomeUsuarioSap(usuarioViewModel);
            await PreencherNomeVendedorSap(usuarioViewModel);

            if (!OperacaoValida()) return null;

            await RemoverMascarasUsuario(usuarioViewModel);

            var usuario = _mapper.Map<Usuario>(usuarioViewModel);

            await _usuarioService.Adicionar(usuario);

            if (!OperacaoValida()) return null;

            if (usuarioViewModel.UsuarioIdentity != null)
            {
                usuarioViewModel.UsuarioIdentity.Id = usuario.Id.ToString();

                if (!await _usuarioIdentityService.CreateUser(usuarioViewModel.UsuarioIdentity, usuarioViewModel.Senha))
                {
                    await _usuarioService.Remover(usuario);

                    return null;
                }
            }

            usuarioViewModel = await ObterUsuarioClaims(usuario.Id);

            if (usuarioViewModel == null)
            {
                Notificar("Não foi possível adicionar o usuário.");
                return null;
            }

            return usuarioViewModel;
        }

        public async Task Atualizar(UsuarioViewModel usuarioViewModel)
        {
            await PreencherNomeUsuarioSap(usuarioViewModel);
            await PreencherNomeVendedorSap(usuarioViewModel);

            if (!OperacaoValida()) return;

            await RemoverMascarasUsuario(usuarioViewModel);

            var usuario = _mapper.Map<Usuario>(usuarioViewModel);

            await _usuarioService.Atualizar(usuario);

            if (usuarioViewModel.UsuarioIdentity != null)
            {
                usuarioViewModel.UsuarioIdentity.Id = usuario.Id.ToString();

                await _usuarioIdentityService.UpdateUser(usuarioViewModel.UsuarioIdentity);
            }
        }

        public async Task Bloquear(Guid id)
        {
            await _usuarioIdentityService.UserDisable(id.ToString());
        }

        public async Task Desbloquear(Guid id)
        {
            await _usuarioIdentityService.UserEnable(id.ToString());
        }

        public async Task AdicionarClaim(ClaimViewModel claimViewModel)
        {
            await _usuarioIdentityService.CreateClaim(claimViewModel.UserId, _mapper.Map<Claim>(claimViewModel));
        }

        public async Task RemoverClaim(ClaimViewModel claimViewModel)
        {
            await _usuarioIdentityService.RemoveClaim(claimViewModel.UserId, _mapper.Map<Claim>(claimViewModel));
        }

        public async Task<UsuarioViewModel> ObterUsuarioAuth()
        {
            var id = await _usuarioIdentityService.GetIdUserAuth();

            return await ObterUsuario(id);
        }

        public async Task<UsuarioViewModel> ObterUsuario(Guid id)
        {
            var usuarioViewModel = _mapper.Map<UsuarioViewModel>(await _usuarioRepository.ObterUsuario(id));

            if (usuarioViewModel.Cargo == null) usuarioViewModel.Cargo = new CargoViewModel();

            return usuarioViewModel;
        }

        public async Task<UsuarioViewModel> ObterUsuarioAuditoria(Guid id)
        {
            return _mapper.Map<UsuarioViewModel>(await _usuarioRepository.ObterUsuarioAuditoria(id));
        }

        public async Task<UsuarioViewModel> ObterUsuarioClaims(Guid id)
        {
            var usuarioViewModel = await ObterUsuario(id);

            if (usuarioViewModel == null) return usuarioViewModel;

            usuarioViewModel.UsuarioIdentity = await _usuarioIdentityService.GetUserById(id.ToString());

            if (usuarioViewModel.UsuarioIdentity == null) usuarioViewModel.UsuarioIdentity = new UsuarioIdentityViewModel();

            usuarioViewModel.Claims = _mapper.Map<List<ClaimViewModel>>(await _usuarioIdentityService.GetClaimsByUser(usuarioViewModel.UsuarioIdentity));

            return usuarioViewModel;
        }

        public async Task<UsuarioViewModel> ObterUsuarioPorUsuarioSap(int usuarioSapId)
        {
            return _mapper.Map<UsuarioViewModel>(await _usuarioRepository.ObterUsuarioPorUsuarioSap(usuarioSapId));
        }

        public async Task<UsuarioViewModel> ObterUsuarioPorVendedorSap(int vendedorSapId)
        {
            return _mapper.Map<UsuarioViewModel>(await _usuarioRepository.ObterUsuarioPorVendedorSap(vendedorSapId));
        }

        public async Task<List<UsuarioViewModel>> ObterUsuarios()
        {
            var usuariosViewModel = _mapper.Map<List<UsuarioViewModel>>(await _usuarioRepository.ObterUsuarios());

            foreach (var usuarioViewModel in usuariosViewModel)
            {
                usuarioViewModel.UsuarioIdentity = await _usuarioIdentityService.GetUserById(usuarioViewModel.Id.ToString());

                if (usuarioViewModel.UsuarioIdentity == null) usuarioViewModel.UsuarioIdentity = new UsuarioIdentityViewModel();

                if (usuarioViewModel.Cargo == null) usuarioViewModel.Cargo = new CargoViewModel();
            }

            return usuariosViewModel;
        }

        public async Task<UsuarioIdentityViewModel> ObterUsuarioIdentity(string userId)
        {
            return await _usuarioIdentityService.GetUserById(userId);
        }

        public async Task<List<ClaimViewModel>> ObterClaims(string userId)
        {
            var usuarioIdentityViewModel = await ObterUsuarioIdentity(userId);

            if (usuarioIdentityViewModel == null) return null;

            return _mapper.Map<List<ClaimViewModel>>(await _usuarioIdentityService.GetClaimsByUser(usuarioIdentityViewModel));
        }

        public async Task<List<ClaimViewModel>> ObterClaimsDisponiveis()
        {
            return _mapper.Map<List<ClaimViewModel>>(await _usuarioIdentityService.GetAllClaimsApp());
        }

        public async Task<List<UsuarioSapViewModel>> ObterUsuariosSap()
        {
            return _mapper.Map<List<UsuarioSapViewModel>>(await _usuarioSapService.ObterTodos());
        }

        private async Task<UsuarioViewModel> PreencherNomeUsuarioSap(UsuarioViewModel usuarioViewModel)
        {
            if (usuarioViewModel.UsuarioSapId == null) return usuarioViewModel;

            var usuarioSap = await _usuarioSapService.ObterUsuario((int)usuarioViewModel.UsuarioSapId);

            if (usuarioSap == null)
            {
                Notificar("Não foi possível obter os dados do usuário selecionado.");

                return usuarioViewModel;
            }

            usuarioViewModel.UsuarioSapNome = usuarioSap.UserName;

            return usuarioViewModel;
        }

        private async Task<UsuarioViewModel> PreencherNomeVendedorSap(UsuarioViewModel usuarioViewModel)
        {
            if (usuarioViewModel.VendedorSapId == null) return usuarioViewModel;

            var vendedorSap = await _vendedorSapService.ObterVendedor((int)usuarioViewModel.VendedorSapId);

            if (vendedorSap == null)
            {
                Notificar("Não foi possível obter os dados do vendedor selecionado.");

                return usuarioViewModel;
            }

            usuarioViewModel.VendedorSapNome = vendedorSap.SalesEmployeeName;

            return usuarioViewModel;
        }

        private Task<UsuarioViewModel> RemoverMascarasUsuario(UsuarioViewModel usuarioViewModel)
        {
            if (usuarioViewModel.UsuarioIdentity != null)
            {
                usuarioViewModel.UsuarioIdentity.PhoneNumber = TratarTexto.SomenteNumeros(usuarioViewModel.UsuarioIdentity.PhoneNumber);
            }

            return Task.FromResult(usuarioViewModel);
        }

        public void Dispose()
        {
            _usuarioService?.Dispose();
            _usuarioRepository?.Dispose();
            _usuarioSapService?.Dispose();
            _vendedorSapService?.Dispose();
            _usuarioIdentityService?.Dispose();
        }
    }
}
