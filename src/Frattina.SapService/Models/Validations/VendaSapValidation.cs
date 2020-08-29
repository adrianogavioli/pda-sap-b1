using FluentValidation;
using System.Linq;

namespace Frattina.SapService.Models.Validations
{
    public class VendaSapValidation : AbstractValidator<VendaSap>
    {
        public VendaSapValidation()
        {
            RuleFor(v => v.DocDate)
                .NotEmpty().WithMessage("O campo Data da NFe é obrigatório.");

            RuleFor(v => v.CardCode)
                .NotEmpty().WithMessage("O campo Código do Cliente é obrigatório.");

            RuleFor(v => v.CardName)
                .NotEmpty().WithMessage("O campo Nome do Cliente é obrigatório.");

            RuleFor(v => v.SalesPersonCode)
                .GreaterThan(0).WithMessage("O campo Vendedor é obrigatório.");

            RuleFor(v => v.DocumentLines.Count())
                .GreaterThan(0).WithMessage("A NFe deve possuir ao menos um item.");
        }
    }
}
