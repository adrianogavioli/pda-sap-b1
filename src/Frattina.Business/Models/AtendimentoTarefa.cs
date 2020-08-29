using Frattina.Business.Models.Enums;
using System;

namespace Frattina.Business.Models
{
    public class AtendimentoTarefa : Entity
    {
        public TarefaTipo Tipo { get; set; }

        public string Assunto { get; set; }

        public DateTime DataTarefa { get; set; }

        public DateTime? DataFinalizacao{ get; set; }

        public bool Removida { get; set; }

        public Guid AtendimentoId { get; set; }

        public Atendimento Atendimento { get; set; }
    }
}
