using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frattina.Application.ViewModels
{
    public class CargoViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 3)]
        [DisplayName( "Cargo")]
        public string Descricao { get; set; }

        public List<UsuarioViewModel> Usuarios { get; set; }
    }
}
