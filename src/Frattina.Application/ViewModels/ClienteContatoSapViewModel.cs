using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frattina.Application.ViewModels
{
    public class ClienteContatoSapViewModel
    {
        public int Id { get; set; }

        public string ClienteId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Identificação")]
        public string Identificacao { get; set; }

        [DisplayName("Telefone")]
        public string Telefone1 { get; set; }

        [DisplayName("Telefone")]
        public string Telefone2 { get; set; }

        [DisplayName("Telefone")]
        public string Telefone3 { get; set; }

        [EmailAddress(ErrorMessage = "O campo {0} não está no formato correto")]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [DisplayName("Observação")]
        public string Observacao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Nome { get; set; }

        public string Sobrenome { get; set; }
    }
}
