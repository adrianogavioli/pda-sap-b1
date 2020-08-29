using Frattina.Application.ViewModels;
using Frattina.CrossCutting.UsuarioIdentity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frattina.Application.Interfaces
{
    public interface IUsuarioApplication : IDisposable
    {
        Task<UsuarioViewModel> Adicionar(UsuarioViewModel usuarioViewModel);

        Task Atualizar(UsuarioViewModel usuarioViewModel);

        Task Bloquear(Guid id);

        Task Desbloquear(Guid id);

        Task AdicionarClaim(ClaimViewModel claimViewModel);

        Task RemoverClaim(ClaimViewModel claimViewModel);

        Task<UsuarioViewModel> ObterUsuarioAuth();

        Task<UsuarioViewModel> ObterUsuario(Guid id);

        Task<UsuarioViewModel> ObterUsuarioAuditoria(Guid id);

        Task<UsuarioViewModel> ObterUsuarioClaims(Guid id);

        Task<UsuarioViewModel> ObterUsuarioPorUsuarioSap(int usuarioSapId);

        Task<UsuarioViewModel> ObterUsuarioPorVendedorSap(int VendedorSapId);

        Task<List<UsuarioViewModel>> ObterUsuarios();

        Task<UsuarioIdentityViewModel> ObterUsuarioIdentity(string userId);

        Task<List<ClaimViewModel>> ObterClaims(string userId);

        Task<List<ClaimViewModel>> ObterClaimsDisponiveis();

        Task<List<UsuarioSapViewModel>> ObterUsuariosSap();
    }
}
