using FluentValidation;

namespace Frattina.Business.Models.Validations
{
    public class AtendimentoProdutoValidation : AbstractValidator<AtendimentoProduto>
    {
        public AtendimentoProdutoValidation()
        {
            RuleFor(a => a.ProdutoSapId)
                .NotEmpty().WithMessage("O campo Código do Produto é obrigatório");

            RuleFor(a => a.Tipo)
                .NotEmpty().WithMessage("O campo Tipo do Produto é obrigatório");

            RuleFor(a => a.Marca)
                .NotEmpty().WithMessage("O campo Marca do Produto é obrigatório");

            RuleFor(a => a.Modelo)
                .NotEmpty().WithMessage("O campo Modelo do Produto é obrigatório");

            RuleFor(a => a.Referencia)
                .NotEmpty().WithMessage("O campo Referência do Produto é obrigatório");

            RuleFor(a => a.Descricao)
                .NotEmpty().WithMessage("O campo Descrição do Produto é obrigatório");

            RuleFor(a => a.Imagem)
                .NotEmpty().WithMessage("O campo Imagem do Produto é obrigatório");

            RuleFor(a => a.ValorTabela)
                .GreaterThan(0).WithMessage("O campo Valor do Produto é obrigatório");

            RuleFor(a => a.ValorNegociado)
                .GreaterThan(0).WithMessage("O campo Valor do Produto é obrigatório");
        }
    }
}
