using System;

namespace Frattina.Application.ViewModels
{
    public class VendedorVisaoViewModel
    {
        public int VendedorId { get; set; }

        public int QuantidadeAtendimentos { get; set; }

        public int QuantidadeVendas { get; set; }

        public decimal TaxaConversaoVenda { get; set; }

        public decimal ValorVendas { get; set; }

        public decimal TicketMedio { get; set; }

        public DateTime? DataUltimoAtendimento { get; set; }

        public string ClienteUltimoAtendimento { get; set; }
    }
}
