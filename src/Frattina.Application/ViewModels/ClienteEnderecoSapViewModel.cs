using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frattina.Application.ViewModels
{
    public class ClienteEnderecoSapViewModel
    {
        public string ClienteId { get; set; }

        public int NumeroLinha { get; set; }

        [DisplayName("Identificação")]
        public string Identificacao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Logradouro { get; set; }

        public string Bairro { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("CEP")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("País")]
        public string Pais { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Estado { get; set; }

        public string Complemento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Número")]
        public string LogradouroNumero { get; set; }

        public bool EnderecoExterior { get; set; }
    }
}
