using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChurrasAPI.Models.Requests
{
    public class ConfirmarParticipacaoRequest : ParticipacaoRequest
    {
        /// <summary>
        /// Valor de contribuição do usuário
        /// </summary>
        [Required]
        public double Contribuicao { get; set; }
    }
}
