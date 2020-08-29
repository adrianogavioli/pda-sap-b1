using FluentValidation;
using System;

namespace Frattina.Business.Models.Validations
{
    public class UsuarioProdutoConsultaValidation : AbstractValidator<UsuarioProdutoConsulta>
    {
        public UsuarioProdutoConsultaValidation()
        {
            RuleFor(u => u.UsuarioId)
                .NotEqual(Guid.Empty).WithMessage("O campo identificação do Usuário é obrigatório");

            RuleFor(u => u.ProdutoId)
                .NotEmpty().WithMessage("O campo identificação do Produto é obrigatório");

            RuleFor(u => u.Imagem)
                .NotEmpty().WithMessage("O campo imagem do Produto é obrigatório");
        }
    }
}
