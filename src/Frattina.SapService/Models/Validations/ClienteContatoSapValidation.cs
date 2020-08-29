using FluentValidation;

namespace Frattina.SapService.Models.Validations
{
    public class ClienteContatoSapValidation : AbstractValidator<ClienteContatoSap>
    {
        public ClienteContatoSapValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("O campo Identificação é obrigatório.")
                .Length(3, 50).WithMessage("O campo Identificação deve ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(c => c.FirstName)
                .NotEmpty().WithMessage("O campo Nome é obrigatório.")
                .Length(3, 50).WithMessage("O campo Nome deve ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(c => c.LastName)
                .Length(3, 50).WithMessage("O campo Sobrenome deve ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(c => c.E_Mail)
                .EmailAddress().WithMessage("O campo E-mail é inválido.");
        }
    }
}
