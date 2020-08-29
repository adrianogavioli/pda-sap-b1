namespace Frattina.SapService.Models
{
    public class ProdutoSap
    {
        public ItemSap Items { get; set; }

        public ProdutoGrupoSap ItemGroups { get; set; }

        public ProdutoTipoSap U_FRTTIPO { get; set; }

        public ProdutoMarcaSap U_FRTMARCA { get; set; }

        public ProdutoModeloSap U_FRTMODELO { get; set; }

        public ProdutoFotoItemSap U_FRTFOTOITEM { get; set; }
    }

    public class ItemSap
    {
        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string SupplierCatalogNo { get; set; }

        public string U_REFINTERNA { get; set; }

        public string U_SEMI { get; set; }
    }
}
