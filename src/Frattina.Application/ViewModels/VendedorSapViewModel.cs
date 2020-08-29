using System.ComponentModel;

namespace Frattina.Application.ViewModels
{
    public class VendedorSapViewModel
    {
        [DisplayName("Código")]
        public int Id { get; set; }

        [DisplayName("Vendedor")]
        public string Nome { get; set; }

        public string Bloqueado { get; set; }
    }
}
