using System.ComponentModel;

namespace Frattina.Application.ViewModels
{
    public class ClienteGrupoSapViewModel
    {
        [DisplayName("Código")]
        public int Id { get; set; }

        [DisplayName("Grupo")]
        public string Nome { get; set; }

        public string Tipo { get; set; }
    }
}
