using System.ComponentModel;

namespace Frattina.Application.ViewModels
{
    public class ProdutoFichaTecnicaSapViewModel
    {
        public int CaracteristicaId { get; set; }

        [DisplayName("Característica")]
        public string Caracteristica { get; set; }

        public int DetalheId { get; set; }

        public string Detalhe { get; set; }

        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [DisplayName("Unidade de Medida")]
        public string UnidadeMedida { get; set; }

        public decimal? Quantidade { get; set; }
    }
}
