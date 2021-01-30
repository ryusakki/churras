using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChurrasAPI.Models
{
    public class Evento
    {
        [Required]
        [Range(1, 31, ErrorMessage = "O dia deve ser especificado no intervalo [1, 31]")]
        public int Dia { get; set; }

        [Required]
        [Range(1, 12, ErrorMessage = "O mês deve ser especificado no intervalo [1, 12]")]
        public int Mes { get; set; }

        [Required]
        public int Ano { get; set; }

        public override bool Equals(object obj) =>
            obj is Evento c && c.Dia == Dia && c.Mes == Mes && c.Ano == Ano;

        public override int GetHashCode() => base.GetHashCode();
    }
}
