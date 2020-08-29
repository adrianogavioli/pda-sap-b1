using System;

namespace Frattina.Application.ViewModels
{
    public class RelUsuarioEmpresaViewModel
    {
        public Guid Id { get; set; }

        public Guid UsuarioId { get; set; }

        public int EmpresaId { get; set; }

        public string EmpresaRazaoSocial { get; set; }

        public string EmpresaNomeFantasia { get; set; }

        public bool Removido { get; set; }

        public UsuarioViewModel Usuario { get; set; }
    }
}
