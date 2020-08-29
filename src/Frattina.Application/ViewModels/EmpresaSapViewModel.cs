using System.ComponentModel;

namespace Frattina.Application.ViewModels
{
    public class EmpresaSapViewModel
    {
        public int Id { get; set; }

        [DisplayName("Razão Social")]
        public string RazaoSocial { get; set; }

        public string Inativa { get; set; }

        [DisplayName("CNPJ")]
        public string Cnpj { get; set; }

        public string TipoEndereco { get; set; }

        public string Logradouro { get; set; }

        [DisplayName("Número")]
        public string LogradouroNumero { get; set; }

        public string Complemento { get; set; }

        [DisplayName("CEP")]
        public string Cep { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Estado { get; set; }

        [DisplayName("País")]
        public string Pais { get; set; }

        [DisplayName("Nome Fantasia")]
        public string NomeFantasia { get; set; }

        public bool Selecionada { get; set; }
    }
}
