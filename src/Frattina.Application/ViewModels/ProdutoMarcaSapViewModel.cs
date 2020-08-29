using System.ComponentModel;

namespace Frattina.Application.ViewModels
{
    public class ProdutoMarcaSapViewModel
    {
        public int Id { get; set; }

        [DisplayName("Marca")]
        public string Nome { get; set; }
    }
}
