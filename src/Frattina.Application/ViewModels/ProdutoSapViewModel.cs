using System.Collections.Generic;
using System.ComponentModel;

namespace Frattina.Application.ViewModels
{
    public class ProdutoSapViewModel
    {
        [DisplayName("Código do Item")]
        public string Id { get; set; }

        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [DisplayName("Referência")]
        public string Referencia { get; set; }

        [DisplayName("Código Master")]
        public string CodigoMaster { get; set; }

        [DisplayName("Semi Novo")]
        public string SemiNovo { get; set; }

        public string Imagem { get; set; }

        public ProdutoGrupoSapViewModel Grupo { get; set; }

        public ProdutoTipoSapViewModel Tipo { get; set; }

        public ProdutoMarcaSapViewModel Marca { get; set; }

        public ProdutoModeloSapViewModel Modelo { get; set; }

        public List<ProdutoEstoqueSapViewModel> Estoques { get; set; }

        public List<ProdutoPrecoSapViewModel> Precos { get; set; }

        public List<ProdutoFotoSapViewModel> Fotos { get; set; }

        public List<ProdutoFichaTecnicaSapViewModel> FichaTecnica { get; set; }
    }
}
