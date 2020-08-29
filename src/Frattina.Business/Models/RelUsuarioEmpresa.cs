using System;

namespace Frattina.Business.Models
{
    public class RelUsuarioEmpresa : Entity
    {
        public Guid UsuarioId { get; set; }

        public int EmpresaId { get; set; }

        public string EmpresaRazaoSocial { get; set; }

        public string EmpresaNomeFantasia { get; set; }

        public bool Removido { get; set; }

        public Usuario Usuario { get; set; }
    }
}
