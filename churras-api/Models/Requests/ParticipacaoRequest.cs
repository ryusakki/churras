using System.ComponentModel.DataAnnotations;

namespace ChurrasAPI.Models.Requests
{
    public class ParticipacaoRequest
    {
        /// <summary>
        /// Data do churrasco / evento
        /// </summary>
        [Required]
        public Evento Evento { get; set; }

        /// <summary>
        /// Endereço de email do usuário
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
        public string UsuarioEmail { get; set; }
    }
}
