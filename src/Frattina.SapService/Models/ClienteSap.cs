using System;
using System.Collections.Generic;

namespace Frattina.SapService.Models
{
    public class ClienteSap
    {
        public string CardCode { get; set; }

        public string CardName { get; set; }

        public string CardType { get; set; }

        public int GroupCode { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string Cellular { get; set; }

        public string EmailAddress { get; set; }

        public int Series { get; set; }

        public string AliasName { get; set; }

        public string ShipToDefault { get; set; }

        public string BilltoDefault { get; set; }

        public string Fax { get; set; } // Utilizado para definir PF ou PJ

        public string U_SAUDACAO { get; set; }

        public DateTime? U_DATACASAMENTO { get; set; }

        public DateTime? U_DATANASCIMENTO { get; set; }

        public int? U_FRTPODERCOMPRACODE { get; set; }

        public string U_SEXO { get; set; }

        public string U_CONJUGE { get; set; }

        public IEnumerable<ClienteEnderecoSap> BPAddresses { get; set; }

        public IEnumerable<ClienteContatoSap> ContactEmployees { get; set; }

        public IEnumerable<ClienteFiscalSap> BPFiscalTaxIDCollection { get; set; }
    }

    public class ClienteSapCrossJoin
    {
        public Businesspartners BusinessPartners { get; set; }

        public class Businesspartners
        {
            public string CardCode { get; set; }
        }
    }
}
