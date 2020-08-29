using Frattina.App.Extensions;
using Frattina.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.App.Controllers
{
    [Authorize]
    public class AuditoriasController : Controller
    {
        private readonly IAuditoriaApplication _auditoriaApplication;

        public AuditoriasController(IAuditoriaApplication auditoriaApplication)
        {
            _auditoriaApplication = auditoriaApplication;
        }

        [ClaimsAuthorize("auditoria", "n1")]
        public async Task<IActionResult> Auditoria(string tabela, Guid chave)
        {
            var auditoriasViewModel = await _auditoriaApplication.ObterAuditorias(tabela, chave);

            TempData["AuditoriaRouteController"] = "Auditorias";
            TempData["AuditoriaRouteChave"] = chave.ToString();

            return PartialView("_Auditoria", auditoriasViewModel.OrderBy(a => a.Data).ToList());
        }

        [ClaimsAuthorize("auditoria", "n1")]
        public async Task<IActionResult> AuditoriaDetalhes(Guid id)
        {
            return PartialView("_AuditoriaDetalhes", await _auditoriaApplication.ObterAuditoria(id));
        }
    }
}
