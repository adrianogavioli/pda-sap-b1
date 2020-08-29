using System.Collections.Generic;

namespace Frattina.Application.ViewModels
{
    public class ProdutoConsultaRapidaViewModel
    {
        public ProdutoSapViewModel ProdutoConsulta { get; set; }

        public List<UsuarioProdutoConsultaViewModel> UsuarioProdutosConsultas { get; set; }
    }
}
