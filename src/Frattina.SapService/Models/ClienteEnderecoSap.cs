using Frattina.SapService.Models.Enums;
using System.Collections.Generic;

namespace Frattina.SapService.Models
{
    public class ClienteEnderecoSap
    {
        public string BPCode { get; set; }

        public int RowNum { get; set; }

        public string AddressType { get; set; }

        public string AddressName { get; set; }

        public string Street { get; set; }

        public string Block { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string BuildingFloorRoom { get; set; }

        public string StreetNo { get; set; }
    }

    public class ClienteEnderecoSapCollection
    {
        public List<ClienteEnderecoSap> BPAddresses { get; set; }
    }
}
