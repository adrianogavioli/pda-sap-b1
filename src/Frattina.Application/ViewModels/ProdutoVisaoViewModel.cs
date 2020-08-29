using System;

namespace Frattina.Application.ViewModels
{
    public class ProdutoVisaoViewModel
    {
        public string ProdutoId { get; set; }

        public int QuantidadeAtendimentos { get; set; }

        public int QuantidadeVendas { get; set; }

        public int QuantidadeNaoGostou { get; set; }

        public int QuantidadeGostou { get; set; }

        public int QuantidadeAmou { get; set; }

        public DateTime? DataUltimoAtendimento { get; set; }

        public string VendedorUltimoAtendimento { get; set; }
    }
}
