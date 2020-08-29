using Frattina.Application.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frattina.Application.ViewModels
{
    public class ClienteSapViewModel
    {
        [DisplayName("Código")]
        public string Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string Nome { get; set; }

        public ClienteTipo Tipo { get; set; }

        public bool Contribuinte { get; set; }

        [DisplayName("Telefone")]
        public string Telefone1 { get; set; }

        [DisplayName("Telefone")]
        public string Telefone2 { get; set; }

        [DisplayName("Telefone")]
        public string Telefone3 { get; set; }

        [EmailAddress(ErrorMessage = "O campo {0} não está no formato correto")]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        public int SerieCodigo { get; set; }

        [DisplayName("Nome Fantasia")]
        public string NomeFantasia { get; set; }

        public string IdentificacaoEnderecoEntrega { get; set; }

        public string IdentificacaoEnderecoCobranca { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Tipo de Pessoa")]
        public string TipoPessoa { get; set; }

        [DisplayName("Saudação")]
        public string Saudacao { get; set; }

        [DisplayName("Data de Casamento")]
        public DateTime? DataCasamento { get; set; }

        [DisplayName("Data de Nascimento")]
        public DateTime? DataNascimento { get; set; }

        [DisplayName("Poder de Compra")]
        public int? PoderCompraId { get; set; }

        [DisplayName("Gênero")]
        public ClienteGenero? Genero { get; set; }

        public string ConjugeId { get; set; }

        [DisplayName("Conjuge")]
        public string ConjugeNome { get; set; }

        public ClienteFiscalSapViewModel DadosFiscais { get; set; }

        public List<ClienteEnderecoSapViewModel> Enderecos { get; set; }

        public List<ClienteContatoSapViewModel> Contatos { get; set; }

        public ClienteGrupoSapViewModel Grupo { get; set; }

        public List<ClienteGrupoSapViewModel> GruposDropDown { get; set; }
    }
}
