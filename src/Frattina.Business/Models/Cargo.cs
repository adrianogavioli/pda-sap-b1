using System.Collections.Generic;

namespace Frattina.Business.Models
{
    public class Cargo : Entity
    {
        public string Descricao { get; set; }

        public IEnumerable<Usuario> Usuarios { get; set; }
    }
}
