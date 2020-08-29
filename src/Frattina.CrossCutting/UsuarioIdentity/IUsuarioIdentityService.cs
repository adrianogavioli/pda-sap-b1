using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Frattina.CrossCutting.UsuarioIdentity
{
    public interface IUsuarioIdentityService : IDisposable
    {
        Task<bool> CreateUser(UsuarioIdentityViewModel usuarioIdentity, string senha);

        Task<bool> UpdateUser(UsuarioIdentityViewModel usuarioIdentity);

        Task<bool> CreateClaim(string userId, Claim claim);

        Task<bool> RemoveClaim(string userId, Claim claim);

        Task<bool> CreateClaimUserAuth(Claim claim);

        Task<bool> RemoveClaimUserAuth(Claim claim);

        Task<List<UsuarioIdentityViewModel>> GetUsers();

        Task<UsuarioIdentityViewModel> GetUserById(string userId);

        Task<UsuarioIdentityViewModel> GetUserByName(string userName);

        Task<IEnumerable<Claim>> GetClaimsByUser(UsuarioIdentityViewModel usuarioIdentity);

        Task<IEnumerable<Claim>> GetClaimsUserAuth();

        Task<Guid> GetIdUserAuth();

        Task UserEnable(string userId);

        Task UserDisable(string userId);

        Task<List<Claim>> GetAllClaimsApp();
    }
}
