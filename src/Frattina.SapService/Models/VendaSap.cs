using System;
using System.Collections.Generic;

namespace Frattina.SapService.Models
{
    public class VendaSap
    {
        public int DocEntry { get; set; }

        public int DocNum { get; set; }

        public DateTime DocDate { get; set; }

        public int BPL_IDAssignedToInvoice { get; set; }

        public string BPLName { get; set; }

        public string VATRegNum { get; set; }

        public string CardCode { get; set; }

        public string CardName { get; set; }

        public decimal DocTotal { get; set; }

        public string DocCurrency { get; set; }

        public string DocTime { get; set; }

        public int SalesPersonCode { get; set; }

        public IEnumerable<VendaItemSap> DocumentLines { get; set; }
    }
}
