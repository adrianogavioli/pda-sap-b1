using FluentValidation;

namespace Frattina.Business.Models.Validations
{
    public class CargoValidation : AbstractValidator<Cargo>
    {
        public CargoValidation()
        {
            RuleFor(c => c.Descricao)
                .NotEmpty().WithMessage("O campo Cargo é obrigatório")
                .Length(3, 100).WithMessage("O campo Cargo deve ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}
