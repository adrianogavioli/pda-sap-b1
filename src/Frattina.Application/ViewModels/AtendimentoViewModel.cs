using Frattina.Business.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frattina.Application.ViewModels
{
    public class AtendimentoViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(1, 999, ErrorMessage = "O campo {0} é inválido")]
        [DisplayName("Empresa")]
        public int EmpresaId { get; set; }

        [DisplayName("Empresa")]
        public string EmpresaNome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Range(1, 999, ErrorMessage = "O campo {0} é inválido")]
        [DisplayName("Vendedor")]
        public int VendedorId { get; set; }

        [DisplayName("Vendedor")]
        public string VendedorNome { get; set; }

        public string ClienteId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 3)]
        [DisplayName("Cliente")]
        public string ClienteNome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Tipo de Pessoa")]
        public string TipoPessoa { get; set; }

        [EmailAddress(ErrorMessage = "O campo {0} não está no formato correto")]
        [DisplayName("E-Mail")]
        public string ClienteEmail { get; set; }

        [DisplayName("Telefone")]
        public string ClienteTelefone { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Data de Nascimento")]
        public DateTime? ClienteNiver { get; set; }

        public bool Contribuinte { get; set; }

        [EnumDataType(typeof(AtendimentoEtapa), ErrorMessage = "O campo {0} é obrigatório")]
        public AtendimentoEtapa Etapa { get; set; }

        [StringLength(1000, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 10)]
        [DisplayName("Negociação")]
        public string Negociacao { get; set; }

        public DateTime Data { get; set; }

        public string ClienteIdVenda { get; set; }

        [StringLength(100, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 3)]
        [DisplayName("Cliente")]
        public string ClienteNomeVenda { get; set; }

        [DisplayName("Valor Total Tabela")]
        public decimal ValorTotalTabela { get; set; }

        [DisplayName("Valor Total Negociado")]
        public decimal ValorTotalNegociado { get; set; }

        [DisplayName("Desconto Total")]
        public decimal PercentTotalDesconto { get; set; }

        public List<AtendimentoProdutoViewModel> Produtos { get; set; }

        public List<AtendimentoTarefaViewModel> Tarefas { get; set; }

        public AtendimentoEncerradoViewModel Encerrado { get; set; }

        public AtendimentoVendidoViewModel Vendido { get; set; }

        public List<EmpresaSapViewModel> EmpresasDropdown { get; set; }

        public List<VendedorSapViewModel> VendedoresDropdown { get; set; }

        public List<AtendimentoProdutoViewModel> ProdutosAmados { get; set; }
    }
}
