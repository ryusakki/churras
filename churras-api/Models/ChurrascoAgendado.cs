using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace ChurrasAPI.Models
{
    public class ChurrascoAgendado : Churrasco
    {
        public ChurrascoAgendado() { }
        public ChurrascoAgendado(Churrasco churrasco)
        {
            Dia = churrasco.Dia;
            Mes = churrasco.Mes;
            Ano = churrasco.Ano;
            Descricao = churrasco.Descricao;
            ObservacoesAdicionais = churrasco.ObservacoesAdicionais;
        }

        [JsonIgnore]
        public virtual ICollection<Participacao> Participacoes { get; set; } = new HashSet<Participacao>();
        public int TotalParticipantes { get => Participacoes.Count; }
        public double ValorArrecadado { get => Participacoes.Sum(p => p.Contribuicao); }
    }
}
