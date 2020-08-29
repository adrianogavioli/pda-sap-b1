using System;

namespace Frattina.Business.Models
{
    public class AtendimentoVendido : Entity
    {
        public int VendaCodigo { get; set; }

        public DateTime Data { get; set; }

        public Guid AtendimentoId { get; set; }

        public Atendimento Atendimento { get; set; }
    }
}
