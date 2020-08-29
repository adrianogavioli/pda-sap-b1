namespace Frattina.SapService.Models
{
    public class ProdutoFotoSap
    {
        public ProdutoFotoItemSap U_FRTFOTOITEM { get; set; }

        public ProdutoFotoGrupoSap U_FRTGRUPOFOTOSITEM { get; set; }
    }

    public class ProdutoFotoItemSap
    {
        public int Code { get; set; }

        public string U_IMG { get; set; }

        public string U_ITEMCODE { get; set; }

        public string U_PRINCIPAL { get; set; }
    }

    public class ProdutoFotoGrupoSap
    {
        public string Name { get; set; }
    }
}
