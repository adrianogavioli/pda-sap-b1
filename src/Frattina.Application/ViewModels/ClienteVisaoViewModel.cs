using System;
using System.Collections.Generic;

namespace Frattina.Application.ViewModels
{
    public class ClienteVisaoViewModel
    {
        public string ClienteId { get; set; }

        public int QuantidadeAtendimentos { get; set; }

        public int QuantidadeVendas { get; set; }

        public decimal TaxaConversaoVenda { get; set; }

        public decimal ValorCompras { get; set; }

        public decimal TicketMedio { get; set; }

        public int Idade { get; set; }

        public int ContagemDiasNiver { get; set; }

        public DateTime? DataUltimoAtendimento { get; set; }

        public string VendedorUltimoAtendimento { get; set; }

        public List<AtendimentoProdutoViewModel> ProdutosAmados { get; set; }
    }
}
