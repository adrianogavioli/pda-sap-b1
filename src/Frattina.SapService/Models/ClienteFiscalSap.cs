namespace Frattina.SapService.Models
{
    public class ClienteFiscalSap
    {
        public string BPCode { get; set; }

        public string TaxId0 { get; set; } // CNPJ

        public string TaxId1 { get; set; } // IE

        public string TaxId2 { get; set; } // IEST

        public string TaxId3 { get; set; } // IM

        public string TaxId4 { get; set; } // CPF

        public string TaxId5 { get; set; } // ID ESTRANGEIRO
    }
}
