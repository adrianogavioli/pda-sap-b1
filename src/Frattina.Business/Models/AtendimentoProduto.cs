using System;

namespace Frattina.Business.Models
{
    public class AtendimentoProduto : Entity
    {
        public string ProdutoSapId { get; set; }

        public string Tipo { get; set; }

        public string Marca { get; set; }

        public string Modelo { get; set; }

        public string Referencia { get; set; }

        public string Descricao { get; set; }

        public string Imagem { get; set; }

        public decimal ValorTabela { get; set; }

        public decimal ValorNegociado { get; set; }

        public int? NivelInteresse { get; set; }

        public bool RemovidoAtendimento { get; set; }

        public bool RemovidoVenda { get; set; }

        public Guid AtendimentoId { get; set; }

        public Atendimento Atendimento { get; set; }
    }
}
