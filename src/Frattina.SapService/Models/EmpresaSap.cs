namespace Frattina.SapService.Models
{
    public class EmpresaSap
    {
        public int BPLID { get; set; }

        public string BPLName { get; set; }

        public string Disabled { get; set; }

        public string FederalTaxID { get; set; } // CNPJ

        public string AddressType { get; set; }

        public string Street { get; set; }

        public string StreetNo { get; set; }

        public string Building { get; set; }

        public string ZipCode { get; set; }

        public string Block { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string AliasName { get; set; }
    }
}
