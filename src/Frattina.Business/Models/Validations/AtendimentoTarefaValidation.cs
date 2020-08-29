using FluentValidation;
using System;

namespace Frattina.Business.Models.Validations
{
    public class AtendimentoTarefaValidation : AbstractValidator<AtendimentoTarefa>
    {
        public AtendimentoTarefaValidation()
        {
            RuleFor(a => a.Tipo)
                .NotEmpty().WithMessage("O campo Tipo da Tarefa é obrigatório");

            RuleFor(a => a.Assunto)
                .NotEmpty().WithMessage("O campo Assunto da Tarefa é obrigatório")
                .Length(3, 500).WithMessage("O campo Assunto da Tarefa deve ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(a => a.DataTarefa)
                .NotEmpty().WithMessage("O campo Data da Tarefa é obrigatório");

            RuleFor(a => a.DataTarefa)
                .GreaterThan(DateTime.Now).WithMessage("A data e hora da tarefa não devem ser inferiores ao momento atual.");
        }
    }
}
