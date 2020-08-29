using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frattina.Application.ViewModels
{
    public class ClaimViewModel
    {
        public int id { get; set; }

        public string UserId { get; set; }

        [DisplayName("Tipo")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Type { get; set; }

        [DisplayName("Valor")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Value { get; set; }

        public UsuarioViewModel Usuario { get; set; }
    }
}
