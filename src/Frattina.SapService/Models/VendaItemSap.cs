namespace Frattina.SapService.Models
{
    public class VendaItemSap
    {
        public int LineNum { get; set; }

        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public string Currency { get; set; }

        public decimal DiscountPercent { get; set; }

        public string WarehouseCode { get; set; }

        public string CFOPCode { get; set; }

        public decimal UnitPrice { get; set; }

        public int DocEntry { get; set; }

        public int NCMCode { get; set; }

        public string TaxCode { get; set; }

        public int Usage { get; set; }

        public string AccountCode { get; set; }
    }
}
