using System;

namespace Frattina.Application.ViewModels
{
    public class UsuarioProdutoConsultaViewModel
    {
        public Guid Id { get; set; }

        public Guid UsuarioId { get; set; }

        public string ProdutoId { get; set; }

        public DateTime DataCadastro { get; set; }

        public string Imagem { get; set; }
    }
}
