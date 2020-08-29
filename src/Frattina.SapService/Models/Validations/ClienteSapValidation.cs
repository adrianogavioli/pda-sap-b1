using FluentValidation;

namespace Frattina.SapService.Models.Validations
{
    public class ClienteSapValidation : AbstractValidator<ClienteSap>
    {
        public ClienteSapValidation()
        {
            RuleFor(c => c.CardName)
                .NotEmpty().WithMessage("O campo Nome é obrigatório.")
                .Length(3, 100).WithMessage("O campo Nome deve ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(c => c.CardType)
                .NotEmpty().WithMessage("O campo Tipo é obrigatório.");

            RuleFor(c => c.GroupCode)
                .GreaterThan(0).WithMessage("O campo Grupo é obrigatório.");

            RuleFor(c => c.Series)
                .GreaterThan(0).WithMessage("O campo Série é obrigatório.");

            RuleFor(c => c.Fax)
                .NotEmpty().WithMessage("O campo Tipo de Pessoa é obrigatório.");

            RuleFor(c => c.EmailAddress)
                .EmailAddress().WithMessage("O campo E-mail é inválido.");

            When(c => !string.IsNullOrEmpty(c.U_CONJUGE), () =>
            {
                RuleFor(c => c.U_SAUDACAO)
                    .NotEmpty().WithMessage("O campo Saudação é obrigatório.");
            });
        }
    }
}
