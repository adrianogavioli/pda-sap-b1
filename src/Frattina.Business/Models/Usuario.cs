using Frattina.Business.Models.Enums;
using System;
using System.Collections.Generic;

namespace Frattina.Business.Models
{
    public class Usuario : Entity
    {
        public string Nome { get; set; }

        public UsuarioTipo Tipo { get; set; }

        public Guid? CargoId { get; set; }

        public Cargo Cargo { get; set; }

        public int? UsuarioSapId { get; set; }

        public string UsuarioSapNome { get; set; }

        public int? VendedorSapId { get; set; }

        public string VendedorSapNome { get; set; }

        public IEnumerable<RelUsuarioEmpresa> Empresas { get; set; }

        public IEnumerable<Auditoria> Auditorias { get; set; }

        public IEnumerable<UsuarioProdutoConsulta> ProdutosConsultas { get; set; }
    }
}
