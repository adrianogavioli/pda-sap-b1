using System.ComponentModel;

namespace Frattina.Application.ViewModels
{
    public class ProdutoModeloSapViewModel
    {
        public int Id { get; set; }

        [DisplayName("Modelo")]
        public string Nome { get; set; }
    }
}
