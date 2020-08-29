using Frattina.Business.Models.Enums;
using System;
using System.Collections.Generic;

namespace Frattina.Business.Models
{
    public class Atendimento : Entity
    {
        public int EmpresaId { get; set; }

        public string EmpresaNome { get; set; }

        public int VendedorId { get; set; }

        public string VendedorNome { get; set; }

        public string ClienteId { get; set; }

        public string ClienteNome { get; set; }

        public string TipoPessoa { get; set; }

        public string ClienteEmail { get; set; }

        public string ClienteTelefone { get; set; }

        public DateTime? ClienteNiver { get; set; }

        public bool Contribuinte { get; set; }

        public AtendimentoEtapa Etapa { get; set; }

        public string Negociacao { get; set; }

        public DateTime Data { get; set; }

        public string ClienteIdVenda { get; set; }

        public string ClienteNomeVenda { get; set; }

        public IEnumerable<AtendimentoProduto> Produtos { get; set; }

        public IEnumerable<AtendimentoTarefa> Tarefas { get; set; }

        public AtendimentoEncerrado Encerrado { get; set; }

        public AtendimentoVendido Vendido { get; set; }
    }
}
