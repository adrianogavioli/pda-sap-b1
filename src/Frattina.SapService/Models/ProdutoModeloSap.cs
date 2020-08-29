namespace Frattina.SapService.Models
{
    public class ProdutoModeloSap
    {
        public int Code { get; set; }

        public string Name { get; set; }
    }

    public class ProdutoModeloSapCrossJoin
    {
        public ProdutoModeloSap U_FRTMODELO { get; set; }
    }
}
