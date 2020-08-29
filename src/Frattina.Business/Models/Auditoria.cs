using System;

namespace Frattina.Business.Models
{
    public class Auditoria : Entity
    {
        public DateTime Data { get; set; }

        public string Tabela { get; set; }

        public string Evento { get; set; }

        public Guid Chave { get; set; }

        public string ValorAntigo { get; set; }

        public string ValorAtual { get; set; }

        public Guid UsuarioId { get; set; }

        public Usuario Usuario { get; set; }
    }
}
