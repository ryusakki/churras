using System.ComponentModel.DataAnnotations;

namespace ChurrasAPI.Models
{
    public class Churrasco : Evento
    {
        [Required(AllowEmptyStrings = false)]
        public string Descricao { get; set; }
        public string ObservacoesAdicionais { get; set; }
    }
}
