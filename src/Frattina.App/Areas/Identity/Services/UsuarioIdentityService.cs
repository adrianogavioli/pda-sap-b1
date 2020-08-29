using AutoMapper;
using Frattina.Application.Interfaces;
using Frattina.Application.ViewModels;
using Frattina.Business.Interfaces;
using Frattina.Business.Notificacoes;
using Frattina.CrossCutting.UsuarioIdentity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Frattina.App.Areas.Identity.Services
{
    public class UsuarioIdentityService : IUsuarioIdentityService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAuditoriaApplication _auditoriaApplication;
        private readonly INotificador _notificador;
        private readonly IMapper _mapper;

        public UsuarioIdentityService(
            IHttpContextAccessor accessor,
            UserManager<IdentityUser> userManager,
            IAuditoriaApplication auditoriaApplication,
            INotificador notificador,
            IMapper mapper)
        {
            _accessor = accessor;
            _userManager = userManager;
            _auditoriaApplication = auditoriaApplication;
            _notificador = notificador;
            _mapper = mapper;
        }

        public async Task<bool> CreateUser(UsuarioIdentityViewModel usuarioIdentityViewModel, string senha)
        {
            var identityUser = _mapper.Map<IdentityUser>(usuarioIdentityViewModel);

            var identityResult = await _userManager.CreateAsync(identityUser, senha);

            if (!identityResult.Succeeded)
            {
                _notificador.Handle(new Notificacao($"Não foi possível adicionar o usuário. {string.Join(';', identityResult.Errors.Select(e => e.Description))}."));

                return false;
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);

            await _userManager.ConfirmEmailAsync(identityUser, token);

            return true;
        }

        public async Task<bool> UpdateUser(UsuarioIdentityViewModel usuarioIdentityViewModel)
        {
            var identityUser = await _userManager.FindByIdAsync(usuarioIdentityViewModel.Id.ToString());

            identityUser.UserName = usuarioIdentityViewModel.UserName;
            identityUser.Email = usuarioIdentityViewModel.Email;
            identityUser.PhoneNumber = usuarioIdentityViewModel.PhoneNumber;

            var identityResult = await _userManager.UpdateAsync(identityUser);

            if (!identityResult.Succeeded)
            {
                _notificador.Handle(new Notificacao($"Não foi possível atualizar o usuário. {string.Join(';', identityResult.Errors.Select(e => e.Description))}."));

                return false;
            }

            return true;
        }

        public async Task<bool> CreateClaim(string userId, Claim claim)
        {
            var identityUser = await _userManager.FindByIdAsync(userId);

            if (identityUser == null)
            {
                _notificador.Handle(new Notificacao("Não foi possível carregar as informações do usuário."));

                return false;
            }

            if (await ClaimEstaDuplicada(identityUser, claim)) return false;

            var identityResult = await _userManager.AddClaimAsync(identityUser, claim);

            if (!identityResult.Succeeded)
            {
                _notificador.Handle(new Notificacao("Algo deu errado ao tentar adicionar a claim do usuário."));

                return false;
            }

            await _auditoriaApplication.Adicionar(new AuditoriaViewModel
            {
                Data = DateTime.Now,
                Tabela = "AspNetUserClaims",
                Evento = "Added",
                Chave = Guid.Parse(userId),
                ValorAntigo = string.Empty,
                ValorAtual = JsonConvert.SerializeObject(_mapper.Map<ClaimViewModel>(claim)),
                UsuarioId = Guid.Parse(_accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
            });

            return true;
        }

        public async Task<bool> RemoveClaim(string userId, Claim claim)
        {
            var identityUser = await _userManager.FindByIdAsync(userId);

            if (identityUser == null)
            {
                _notificador.Handle(new Notificacao("Não foi possível carregar as informações do usuário."));

                return false;
            }

            var identityResult = await _userManager.RemoveClaimAsync(identityUser, claim);

            if (!identityResult.Succeeded)
            {
                _notificador.Handle(new Notificacao("Algo deu errado ao tentar remover a claim do usuário."));

                return false;
            }

            await _auditoriaApplication.Adicionar(new AuditoriaViewModel
            {
                Data = DateTime.Now,
                Tabela = "AspNetUserClaims",
                Evento = "Deleted",
                Chave = Guid.Parse(userId),
                ValorAntigo = string.Empty,
                ValorAtual = JsonConvert.SerializeObject(_mapper.Map<ClaimViewModel>(claim)),
                UsuarioId = Guid.Parse(_accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
            });

            return true;
        }

        public async Task<bool> CreateClaimUserAuth(Claim claim)
        {
            if (!_accessor.HttpContext.User.Identity.IsAuthenticated) return false;

            var identityUser = await _userManager.FindByNameAsync(_accessor.HttpContext.User.Identity.Name);

            if (identityUser == null)
            {
                _notificador.Handle(new Notificacao("Não foi possível carregar as informações do usuário."));

                return false;
            }

            var identityResult = await _userManager.AddClaimAsync(identityUser, claim);

            if (!identityResult.Succeeded)
            {
                _notificador.Handle(new Notificacao("Algo deu errado ao tentar adicionar a claim do usuário."));

                return false;
            }

            return true;
        }

        public async Task<bool> RemoveClaimUserAuth(Claim claim)
        {
            if (!_accessor.HttpContext.User.Identity.IsAuthenticated) return false;

            var identityUser = await _userManager.FindByNameAsync(_accessor.HttpContext.User.Identity.Name);

            if (identityUser == null)
            {
                _notificador.Handle(new Notificacao("Não foi possível carregar as informações do usuário."));

                return false;
            }

            var identityResult = await _userManager.RemoveClaimAsync(identityUser, claim);

            if (!identityResult.Succeeded)
            {
                _notificador.Handle(new Notificacao("Algo deu errado ao tentar remover a claim do usuário."));

                return false;
            }

            return true;
        }

        public async Task<List<UsuarioIdentityViewModel>> GetUsers()
        {
            return _mapper.Map<List<UsuarioIdentityViewModel>>(await _userManager.Users.ToListAsync());
        }

        public async Task<UsuarioIdentityViewModel> GetUserById(string userId)
        {
            return _mapper.Map<UsuarioIdentityViewModel>(await _userManager.FindByIdAsync(userId));
        }

        public async Task<UsuarioIdentityViewModel> GetUserByName(string userName)
        {
            return _mapper.Map<UsuarioIdentityViewModel>(await _userManager.FindByNameAsync(userName));
        }

        public async Task<IEnumerable<Claim>> GetClaimsByUser(UsuarioIdentityViewModel usuarioIdentityViewModel)
        {
            return await _userManager.GetClaimsAsync(_mapper.Map<IdentityUser>(usuarioIdentityViewModel));
        }

        public async Task<IEnumerable<Claim>> GetClaimsUserAuth()
        {
            if (!_accessor.HttpContext.User.Identity.IsAuthenticated) return null;

            var identityUser = await _userManager.FindByNameAsync(_accessor.HttpContext.User.Identity.Name);

            return await _userManager.GetClaimsAsync(identityUser);
        }

        public async Task<Guid> GetIdUserAuth()
        {
            if (!_accessor.HttpContext.User.Identity.IsAuthenticated) return Guid.Empty;

            var identityUser = await _userManager.FindByNameAsync(_accessor.HttpContext.User.Identity.Name);

            if (identityUser == null) return Guid.Empty;

            return Guid.Parse(identityUser.Id);
        }

        public async Task UserEnable(string userId)
        {
            var identityUser = await _userManager.FindByIdAsync(userId);

            if (identityUser == null)
            {
                _notificador.Handle(new Notificacao("Não foi possível carregar as informações do usuário."));

                return;
            }

            var identityResult = await _userManager.SetLockoutEndDateAsync(identityUser, null);

            if (!identityResult.Succeeded)
            {
                _notificador.Handle(new Notificacao("Não foi possível desbloquear o acesso do usuário."));

                return;
            }
        }

        public async Task UserDisable(string userId)
        {
            var identityUser = await _userManager.FindByIdAsync(userId);

            if (identityUser == null)
            {
                _notificador.Handle(new Notificacao("Não foi possível carregar as informações do usuário."));

                return;
            }

            var identityResult = await _userManager.SetLockoutEnabledAsync(identityUser, true);

            if (!identityResult.Succeeded)
            {
                _notificador.Handle(new Notificacao("Não foi possível bloquear o acesso do usuário."));

                return;
            }

            identityResult = await _userManager.SetLockoutEndDateAsync(identityUser, new DateTime(2999, 01, 01));

            if (!identityResult.Succeeded)
            {
                _notificador.Handle(new Notificacao("Não foi possível bloquear o acesso do usuário."));

                return;
            }
        }

        private async Task<bool> ClaimEstaDuplicada(IdentityUser identityUser, Claim claim)
        {
            var claims = await _userManager.GetClaimsAsync(identityUser);

            if (claims.ToList().Exists(c => c.Type == claim.Type))
            {
                _notificador.Handle(new Notificacao("O usuário pode possuir apenas uma permissão por tipo."));

                return true;
            }

            return false;
        }

        public Task<List<Claim>> GetAllClaimsApp()
        {
            var claims = new List<Claim>
            {
                new Claim("atendimento","n1"),
                new Claim("atendimento","n1,n2"),
                new Claim("atendimento","n1,n2,n3"),
                new Claim("auditoria","n1"),
                new Claim("cargo","n1"),
                new Claim("cargo","n1,n2"),
                new Claim("cargo","n1,n2,n3"),
                new Claim("cliente","n1"),
                new Claim("cliente","n1,n2"),
                new Claim("cliente","n1,n2,n3"),
                new Claim("produto","n1"),
                new Claim("usuario","n1"),
                new Claim("usuario","n1,n2"),
                new Claim("usuario","n1,n2,n3"),
                new Claim("venda","n1"),
                new Claim("venda","n1,n2"),
                new Claim("venda","n1,n2,n3"),
                new Claim("vendedor","n1")
            };

            return Task.FromResult(claims);
        }

        public void Dispose()
        {
            _userManager?.Dispose();
        }
    }
}
