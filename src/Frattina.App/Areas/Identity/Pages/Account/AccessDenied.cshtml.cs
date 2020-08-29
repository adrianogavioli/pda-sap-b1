using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frattina.App.Areas.Identity.Pages.Account
{
    [Authorize]
    public class AccessDeniedModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}

