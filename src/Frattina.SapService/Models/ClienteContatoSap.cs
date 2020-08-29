using System.Collections.Generic;

namespace Frattina.SapService.Models
{
    public class ClienteContatoSap
    {
        public int InternalCode { get; set; }

        public string CardCode { get; set; }

        public string Name { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string MobilePhone { get; set; }

        public string E_Mail { get; set; }

        public string Remarks1 { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class ClienteContatoSapCollection
    {
        public List<ClienteContatoSap> ContactEmployees { get; set; }
    }
}
