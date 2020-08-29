namespace Frattina.SapService.Models
{
    public class ProdutoMarcaSap
    {
        public int Code { get; set; }

        public string Name { get; set; }
    }

    public class ProdutoMarcaSapCrossJoin
    {
        public ProdutoMarcaSap U_FRTMARCA { get; set; }
    }
}
