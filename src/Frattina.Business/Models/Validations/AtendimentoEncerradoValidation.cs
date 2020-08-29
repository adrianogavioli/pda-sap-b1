using FluentValidation;

namespace Frattina.Business.Models.Validations
{
    public class AtendimentoEncerradoValidation : AbstractValidator<AtendimentoEncerrado>
    {
        public AtendimentoEncerradoValidation()
        {
            RuleFor(a => a.Motivo)
                .NotEmpty().WithMessage("O campo Motivo é obrigatório.");

            RuleFor(a => a.Justificativa)
                .NotEmpty().WithMessage("O campo Justificativa é obrigatório.")
                .Length(10, 1000).WithMessage("O campo Justificativa deve ter entre {MinLength} e {MaxLength} caracteres.");
        }
    }
}
