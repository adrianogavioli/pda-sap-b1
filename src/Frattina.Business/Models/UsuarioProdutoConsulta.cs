using System;

namespace Frattina.Business.Models
{
    public class UsuarioProdutoConsulta : Entity
    {
        public Guid UsuarioId { get; set; }

        public string ProdutoId { get; set; }

        public DateTime DataCadastro { get; set; }

        public string Imagem { get; set; }

        public Usuario Usuario { get; set; }
    }
}
