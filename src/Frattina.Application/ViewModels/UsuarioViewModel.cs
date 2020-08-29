using Frattina.Business.Models.Enums;
using Frattina.CrossCutting.UsuarioIdentity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frattina.Application.ViewModels
{
    public class UsuarioViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public UsuarioTipo Tipo { get; set; }

        [DisplayName("Cargo")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid CargoId { get; set; }

        public CargoViewModel Cargo { get; set; }

        [DisplayName("Usuário SAP")]
        public int? UsuarioSapId { get; set; }

        [DisplayName("Usuário SAP")]
        public string UsuarioSapNome { get; set; }

        [DisplayName("Vendedor SAP")]
        public int? VendedorSapId { get; set; }

        [DisplayName("Vendedor SAP")]
        public string VendedorSapNome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(50, ErrorMessage = "A {0} deve ter entre {2} e {1} caracteres", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Repita a senha")]
        [Compare("Senha", ErrorMessage = "A senha e sua confirmação não conferem")]
        public string ConfirmacaoSenha { get; set; }

        public UsuarioIdentityViewModel UsuarioIdentity { get; set; }

        public List<ClaimViewModel> Claims { get; set; }

        public List<RelUsuarioEmpresaViewModel> Empresas { get; set; }

        public List<CargoViewModel> CargosDropdown { get; set; }

        public List<UsuarioSapViewModel> UsuariosSapDropdown { get; set; }

        public List<VendedorSapViewModel> VendedoresSapDropdown { get; set; }

        public List<EmpresaSapViewModel> EmpresasDropdown { get; set; }

        public List<ClaimViewModel> ClaimsDisponiveis { get; set; }
    }
}
