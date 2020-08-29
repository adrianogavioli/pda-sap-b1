using FluentValidation;
using Frattina.CrossCutting.Documentos;

namespace Frattina.SapService.Models.Validations
{
    public class ClienteFiscalSapValidation : AbstractValidator<ClienteFiscalSap>
    {
        public ClienteFiscalSapValidation()
        {
            When(c => !string.IsNullOrEmpty(c.TaxId4), () =>
            {
                RuleFor(c => c.TaxId4.Length).Equal(CpfValidacao.TamanhoCpf)
                    .WithMessage("O campo CPF precisa ter {ComparisonValue} caracteres.");

                RuleFor(c => CpfValidacao.Validar(c.TaxId4)).Equal(true)
                    .WithMessage("O campo CPF é inválido.");
            });

            When(c => !string.IsNullOrEmpty(c.TaxId0), () =>
            {
                RuleFor(c => c.TaxId0.Length).Equal(CnpjValidacao.TamanhoCnpj)
                    .WithMessage("O campo CNPJ precisa ter {ComparisonValue} caracteres.");

                RuleFor(c => CnpjValidacao.Validar(c.TaxId0)).Equal(true)
                    .WithMessage("O campo CNPJ é inválido.");
            });
        }
    }
}
