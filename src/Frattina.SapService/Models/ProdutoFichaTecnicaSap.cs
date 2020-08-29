namespace Frattina.SapService.Models
{
    public class ProdutoFichaTecnicaSap
    {
        public ProdutoCaracteristicaSap U_FRTCARACTERISTICA { get; set; }

        public ProdutoDetalheSap U_FRTDETALHE { get; set; }

        public ProdutoDetalheValorSap U_FRTCARACDETITEM { get; set; }
    }

    public class ProdutoCaracteristicaSap
    {
        public int Code { get; set; }

        public string Name { get; set; }
    }

    public class ProdutoDetalheSap
    {
        public int Code { get; set; }

        public string Name { get; set; }
    }

    public class ProdutoDetalheValorSap
    {
        public string Name { get; set; }

        public string U_MEDIDA { get; set; }

        public decimal? U_VALOR { get; set; }
    }
}
