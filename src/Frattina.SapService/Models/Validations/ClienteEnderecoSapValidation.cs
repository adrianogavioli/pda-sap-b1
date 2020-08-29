using FluentValidation;
using Frattina.SapService.Models.Enums;

namespace Frattina.SapService.Models.Validations
{
    public class ClienteEnderecoSapValidation : AbstractValidator<ClienteEnderecoSap>
    {
        public ClienteEnderecoSapValidation()
        {
            RuleFor(c => c.BPCode)
                .NotEmpty().WithMessage("O campo Código é obrigatório.");

            RuleFor(c => c.AddressType)
                .IsEnumName(typeof(AddressType)).WithMessage("O campo Tipo é obrigatório.");

            RuleFor(c => c.AddressName)
                .NotEmpty().WithMessage("O campo Identificação é obrigatório.")
                .Length(3, 50).WithMessage("O campo Identificação deve ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(c => c.ZipCode)
                .NotEmpty().WithMessage("O campo CEP é obrigatório.")
                .MaximumLength(20).WithMessage("O campo CEP deve ter no máximo {MaxLength} caracteres.");

            RuleFor(c => c.Street)
                .NotEmpty().WithMessage("O campo Logradouro é obrigatório.")
                .Length(3, 100).WithMessage("O campo Logradouro deve ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(c => c.StreetNo)
                .NotEmpty().WithMessage("O campo Número é obrigatório.")
                .MaximumLength(100).WithMessage("O campo Número deve ter no máximo {MaxLength} caracteres.");

            RuleFor(c => c.BuildingFloorRoom)
                .MaximumLength(20).WithMessage("O campo Complemento deve ter no máximo {MaxLength} caracteres.");

            RuleFor(c => c.Block)
                .MaximumLength(100).WithMessage("O campo Bairro deve ter no máximo {MaxLength} caracteres.");

            RuleFor(c => c.City)
                .NotEmpty().WithMessage("O campo Cidade é obrigatório.")
                .Length(3, 100).WithMessage("O campo Cidade deve ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(c => c.State)
                .NotEmpty().WithMessage("O campo Estado é obrigatório.");

            RuleFor(c => c.Country)
                .NotEmpty().WithMessage("O campo País é obrigatório.");
        }
    }
}
