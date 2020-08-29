using System.ComponentModel;

namespace Frattina.Application.ViewModels
{
    public class ProdutoGrupoSapViewModel
    {
        [DisplayName("Código")]
        public int Id { get; set; }

        [DisplayName("Grupo")]
        public string Nome { get; set; }
    }
}
