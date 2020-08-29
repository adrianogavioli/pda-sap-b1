namespace Frattina.Application.ViewModels
{
    public class EstoqueSapViewModel
    {
        public string Codigo { get; set; }

        public string Nome { get; set; }

        public bool PermiteVenda { get; set; }

        public bool Inativo { get; set; }

        public int EmpresaId { get; set; }
    }
}
