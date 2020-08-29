using System.ComponentModel;

namespace Frattina.Application.ViewModels
{
    public class ClienteFiscalSapViewModel
    {
        public string ClienteId { get; set; }

        [DisplayName("CNPJ")]
        public string Cnpj { get; set; }  

        [DisplayName("Inscrição Estadual")]
        public string InscricaoEstadual { get; set; }  

        [DisplayName("Incrição Estadual Subs. Tributária")]
        public string InscricaoEstadualST { get; set; }  

        [DisplayName("Inscrição Municipal")]
        public string InscricaoMunicipal { get; set; }  

        [DisplayName("CPF")]
        public string Cpf { get; set; }  

        public string Passaporte { get; set; }   
    }
}
