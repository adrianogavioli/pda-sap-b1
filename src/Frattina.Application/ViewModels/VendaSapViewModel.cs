using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Frattina.Application.ViewModels
{
    public class VendaSapViewModel
    {
        public int Id { get; set; }

        [DisplayName("Número da NFe")]
        public int NumeroNf { get; set; }

        [DisplayName("Data de Emissão")]
        public DateTime DataEmissao { get; set; }

        public int EmpresaId { get; set; }

        [DisplayName("Nome da Empresa")]
        public string EmpresaNome { get; set; }

        [DisplayName("CNPJ da Empresa")]
        public string EmpresaCnpj { get; set; }

        public string ClienteId { get; set; }

        [DisplayName("Nome do Cliente")]
        public string ClienteNome { get; set; }

        [DisplayName("Valor Total")]
        public decimal ValorTotal { get; set; }

        public string Moeda { get; set; }

        [DisplayName("Hora da Emissão")]
        public string HoraEmissao { get; set; }

        [DisplayName("Vendedor")]
        public int VendedorId { get; set; }

        public List<VendaItemSapViewModel> Produtos { get; set; }

        public Guid AtendimentoId { get; set; }

        public VendedorSapViewModel Vendedor { get; set; }
    }
}
