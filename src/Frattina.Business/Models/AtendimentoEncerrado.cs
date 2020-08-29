using Frattina.Business.Models.Enums;
using System;

namespace Frattina.Business.Models
{
    public class AtendimentoEncerrado : Entity
    {
        public AtendimentoEncerradoMotivo Motivo { get; set; }

        public string Justificativa { get; set; }

        public DateTime Data { get; set; }

        public Guid AtendimentoId { get; set; }

        public Atendimento Atendimento { get; set; }
    }
}
