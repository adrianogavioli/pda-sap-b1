using FluentValidation;

namespace Frattina.Business.Models.Validations
{
    public class AtendimentoVendidoValidation : AbstractValidator<AtendimentoVendido>
    {
        public AtendimentoVendidoValidation()
        {
            RuleFor(a => a.VendaCodigo)
                .GreaterThan(0).WithMessage("O código da venda é obrigatório.");
        }
    }
}
