using Frattina.Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Frattina.App.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUsuarioApplication _usuarioApplication;

        public LoginModel(
            SignInManager<IdentityUser> signInManager,
            IUsuarioApplication usuarioApplication)
        {
            _signInManager = signInManager;
            _usuarioApplication = usuarioApplication;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage ="O campo {0} deve ser preenchido")]
            [Display(Name = "Usuário")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "O campo {0} deve ser preenchido")]
            [DataType(DataType.Password)]
            [Display(Name = "Senha")]
            public string Password { get; set; }

            [Display(Name = "Lembre-me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (!ModelState.IsValid) return Page();

            var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, false, true);
            if (result.Succeeded)
            {
                var user = await _signInManager.UserManager.FindByNameAsync(Input.UserName);
                var userClaims = await _signInManager.UserManager.GetClaimsAsync(user);
                var usuarioViewModel = await _usuarioApplication.ObterUsuario(Guid.Parse(user.Id));

                var userIdClaim = userClaims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim != null)
                    await _signInManager.UserManager.RemoveClaimAsync(user, userIdClaim);

                await _signInManager.UserManager.AddClaimAsync(user, new Claim("UserId", user.Id));

                var userProfileClaim = userClaims.FirstOrDefault(c => c.Type == "UserProfile");
                if (userProfileClaim != null)
                    await _signInManager.UserManager.RemoveClaimAsync(user, userProfileClaim);

                var userProfile = usuarioViewModel.Nome;
                if (usuarioViewModel.Cargo != null) userProfile += $"#{usuarioViewModel.Cargo.Descricao}";
                await _signInManager.UserManager.AddClaimAsync(user, new Claim("UserProfile", userProfile));

                return LocalRedirect(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = false });
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Usuário ou senha inválido.");
                return Page();
            }
        }
    }
}
