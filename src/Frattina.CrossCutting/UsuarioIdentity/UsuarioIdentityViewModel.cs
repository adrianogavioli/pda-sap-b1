using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frattina.CrossCutting.UsuarioIdentity
{
    public class UsuarioIdentityViewModel
    {
        [Key]
        public string Id { get; set; }

        [DisplayName("Usuário")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        [DisplayName("E-mail")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} não está em um formato válido")]
        public string Email { get; set; }

        public string NormalizedEmail { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string ConcurrencyStamp { get; set; }

        [DisplayName("Telefone")]
        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        [DisplayName("Prazo de Bloqueio")]
        public DateTimeOffset? LockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; }

        [DisplayName("Tentativas Inválidas")]
        public int AccessFailedCount { get; set; }
    }
}
