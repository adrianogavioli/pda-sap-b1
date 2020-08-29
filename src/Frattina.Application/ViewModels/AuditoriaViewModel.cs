using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Frattina.Application.ViewModels
{
    public class AuditoriaViewModel
    {
        public Guid Id { get; set; }

        public DateTime Data { get; set; }

        public string Tabela { get; set; }

        public string Evento { get; set; }

        public Guid Chave { get; set; }

        public string ValorAntigo { get; set; }

        public string ValorAtual { get; set; }

        [DisplayName("Usuário")]
        public Guid UsuarioId { get; set; }

        public UsuarioViewModel Usuario { get; set; }

        public Dictionary<string, string> ValoresAntigos { get; set; }

        public Dictionary<string, string> ValoresAtuais { get; set; }
    }
}
