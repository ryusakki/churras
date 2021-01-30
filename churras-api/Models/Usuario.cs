using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChurrasAPI.Models
{
    /// <summary>
    /// Até daria para herdar da model 'AuthRequest' para reutilizar os campos de Email e senha,
    /// mas acho que não faria muito sentido dizer que um usuário é uma AuthRequest...
    /// </summary>
    public class Usuario
    {
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Não é um email válido")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo 'Senha' é obrigatório.")]
        public string Senha { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo 'Nome' é obrigatório.")]
        public string Nome { get; set; }

        public override bool Equals(object obj) => 
            obj is Usuario u && u.Email == Email;

        public override int GetHashCode() => base.GetHashCode();
    }
}
