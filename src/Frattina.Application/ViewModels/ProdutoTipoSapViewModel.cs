using System.ComponentModel;

namespace Frattina.Application.ViewModels
{
    public class ProdutoTipoSapViewModel
    {
        public int Id { get; set; }

        [DisplayName("Tipo")]
        public string Nome { get; set; }
    }
}
