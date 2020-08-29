using AutoMapper;
using Frattina.Application.Interfaces;
using Frattina.Business.Interfaces;
using Frattina.CrossCutting.StringTools;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Frattina.App.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUsuarioApplication _usuarioApplication;
        private readonly IMapper _mapper;
        private readonly INotificador _notificador;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUsuarioApplication usuarioApplication,
            IMapper mapper,
            INotificador notificador)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _usuarioApplication = usuarioApplication;
            _mapper = mapper;
            _notificador = notificador;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "O {0} deve ser informado")]
            [Display(Name = "Nome")]
            public string Name { get; set; }

            [Phone(ErrorMessage ="O {0} informado é inválido")]
            [Display(Name = "Telefone")]
            public string PhoneNumber { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var usuarioViewModel = await _usuarioApplication.ObterUsuario(Guid.Parse(user.Id));
            if (usuarioViewModel == null) return NotFound();

            Input = new InputModel
            {
                Name = usuarioViewModel.Nome,
                PhoneNumber = user.PhoneNumber
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var usuarioViewModel = await _usuarioApplication.ObterUsuario(Guid.Parse(user.Id));
            if (usuarioViewModel == null) return NotFound();

            Input = new InputModel
            {
                Name = Input.Name,
                PhoneNumber = TratarTexto.SomenteNumeros(Input.PhoneNumber)
            };

            if (!ModelState.IsValid) return Page();

            usuarioViewModel.Nome = Input.Name;

            await _usuarioApplication.Atualizar(usuarioViewModel);

            if (_notificador.TemNotificacao()) return Page();

            if (Input.PhoneNumber != user.PhoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    foreach (var error in setPhoneResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return Page();
                }
            }

            await _signInManager.RefreshSignInAsync(user);

            TempData["Sucesso"] = "Suas informações foram atualizadas.";

            return RedirectToPage();
        }
    }
}
