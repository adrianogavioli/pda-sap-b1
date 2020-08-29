using Frattina.Application.ViewModels;
using Frattina.CrossCutting.UsuarioIdentity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.App.Extensions
{
    public class UserProfileViewComponent : ViewComponent
    {
        private readonly IUsuarioIdentityService _usuarioIdentityService;

        public UserProfileViewComponent(IUsuarioIdentityService usuarioIdentityService)
        {
            _usuarioIdentityService = usuarioIdentityService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userClaims = await _usuarioIdentityService.GetClaimsUserAuth();

            if (userClaims == null) return View(new UsuarioViewModel());

            var userProfileClaim = userClaims.FirstOrDefault(c => c.Type == "UserProfile");

            if (userProfileClaim == null) return View(new UsuarioViewModel());

            var userProfileArray = userProfileClaim.Value.Split("#");

            var usuarioViewModel = new UsuarioViewModel
            {
                Nome = userProfileArray[0],
                Cargo = new CargoViewModel()
            };

            if (userProfileArray.Length > 1)
                usuarioViewModel.Cargo.Descricao = userProfileArray[1];

            return View(usuarioViewModel);
        }
    }
}
