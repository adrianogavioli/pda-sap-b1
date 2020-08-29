using Frattina.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Frattina.App.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IAutenticacaoApplication _authApplication;

        public LogoutModel(SignInManager<IdentityUser> signInManager,
            IAutenticacaoApplication authApplication)
        {
            _signInManager = signInManager;
            _authApplication = authApplication;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            // Logout no App
            await _signInManager.SignOutAsync();

            // Logout no SAP
            await _authApplication.Logout();

            TempData.Clear();

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return Page();
            }
        }
    }
}