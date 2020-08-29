using FluentValidation;
using Frattina.Business.Interfaces.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Frattina.Business.Models.Validations
{
    public class RelUsuarioEmpresaValidation : AbstractValidator<RelUsuarioEmpresa>
    {
        private readonly IRelUsuarioEmpresaRepository _relUsuarioEmpresaRepository;

        public RelUsuarioEmpresaValidation(IRelUsuarioEmpresaRepository relUsuarioEmpresaRepository)
        {
            _relUsuarioEmpresaRepository = relUsuarioEmpresaRepository;

            RuleFor(r => r.UsuarioId)
                .NotEqual(Guid.Empty).WithMessage("O campo Usuário é obrigatório");

            RuleFor(r => r.EmpresaId)
                .NotEqual(0).WithMessage("O campo Empresa é obrigatório");

            RuleFor(r => r.EmpresaRazaoSocial)
                .NotEmpty().WithMessage("O campo Razão Social é obrigatório")
                .NotNull().WithMessage("O campo Razão Social é obrigatório");

            RuleFor(r => r.EmpresaNomeFantasia)
                .NotEmpty().WithMessage("O campo Nome Fantasia é obrigatório")
                .NotNull().WithMessage("O campo Nome Fantasia é obrigatório");

            RuleFor(r => RelacionamentoExistente(r).Result)
                .NotEqual(true).WithMessage("Relação entre usuário e empresa já estabelecida");
        }

        private async Task<bool> RelacionamentoExistente(RelUsuarioEmpresa relUsuarioEmpresa)
        {
            var rel = await _relUsuarioEmpresaRepository.Buscar(r => r.UsuarioId == relUsuarioEmpresa.UsuarioId
                                                                && r.EmpresaId == relUsuarioEmpresa.EmpresaId
                                                                && !r.Removido);

            return rel.Any();
        }
    }
}
