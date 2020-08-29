using FluentValidation;

namespace Frattina.Business.Models.Validations
{
    public class UsuarioValidation : AbstractValidator<Usuario>
    {
        public UsuarioValidation()
        {
            RuleFor(u => u.Nome)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório")
                .Length(3, 200).WithMessage("O campo {PropertyName} deve ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(u => u.Tipo)
                .NotEmpty().WithMessage("O campo Tipo é obrigatório");

            RuleFor(u => u.CargoId)
                .NotEmpty().WithMessage("O campo Cargo é obrigatório");
        }
    }
}
