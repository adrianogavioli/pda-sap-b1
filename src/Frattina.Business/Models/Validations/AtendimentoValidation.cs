using FluentValidation;

namespace Frattina.Business.Models.Validations
{
    public class AtendimentoValidation : AbstractValidator<Atendimento>
    {
        public AtendimentoValidation()
        {
            RuleFor(a => a.EmpresaId)
                .GreaterThan(0).WithMessage("O campo Empresa é obrigatório.");

            RuleFor(a => a.EmpresaNome)
                .NotEmpty().WithMessage("O campo Empresa é obrigatório.")
                .MaximumLength(100).WithMessage("O campo Empresa deve ter no máximo {MaxLength} caracteres.");

            RuleFor(a => a.VendedorId)
                .GreaterThan(0).WithMessage("O campo Vendedor é obrigatório.");

            RuleFor(a => a.VendedorNome)
                .NotEmpty().WithMessage("O campo Vendedor é obrigatório.")
                .MaximumLength(100).WithMessage("O campo Vendedor deve ter no máximo {MaxLength} caracteres.");

            RuleFor(a => a.ClienteId)
                .NotEmpty().WithMessage("O campo Cliente é obrigatório.");

            RuleFor(a => a.ClienteNome)
                .NotEmpty().WithMessage("O campo Cliente é obrigatório.")
                .Length(3, 100).WithMessage("O campo Cliente deve ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(a => a.TipoPessoa)
                .NotEmpty().WithMessage("O campo Tipo de Pessoa é obrigatório.");

            RuleFor(a => a.ClienteEmail)
                .EmailAddress().WithMessage("O campo E-mail não está no formato correto.");

            RuleFor(a => a.Etapa)
                .NotEmpty().WithMessage("O campo Etapa é obrigatório.");

            RuleFor(a => a.Negociacao)
                .Length(10, 1000).WithMessage("O campo Negociação deve ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(a => a.ClienteIdVenda)
                .NotEmpty().WithMessage("O campo Cliente (venda) é obrigatório.");

            RuleFor(a => a.ClienteNomeVenda)
                .NotEmpty().WithMessage("O campo Cliente (venda) é obrigatório.")
                .Length(3, 100).WithMessage("O campo Cliente deve ter entre {MinLength} e {MaxLength} caracteres.");
        }
    }
}
