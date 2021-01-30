using System;
using System.Diagnostics.CodeAnalysis;

namespace ChurrasAPI.Models
{
    /// <summary>
    /// Estrutura que relaciona um usuário com um churrasco
    /// </summary>
    public class Participacao
    {
        public Participacao(){}

        public Participacao(Usuario usuario, ChurrascoAgendado churrascoAgendado)
        {
           UsuarioEmail = usuario.Email;
           Usuario = usuario;
           ChurrascoAgendadoDia = churrascoAgendado.Dia;
           ChurrascoAgendadoMes = churrascoAgendado.Mes;
           ChurrascoAgendadoAno = churrascoAgendado.Ano;
           ChurrascoAgendado = churrascoAgendado;
        }

        public Participacao(Usuario usuario, ChurrascoAgendado churrascoAgendado, double contribuicao) : 
            this(usuario, churrascoAgendado) => Contribuicao = contribuicao;

        public string UsuarioEmail { get; set; }
        public virtual Usuario Usuario { get; set; }
        public double Contribuicao { get; set; }
        public int ChurrascoAgendadoDia { get; set; }
        public int ChurrascoAgendadoMes { get; set; }
        public int ChurrascoAgendadoAno { get; set; }
        public virtual ChurrascoAgendado ChurrascoAgendado { get; set; }

        public override bool Equals(object obj) =>
            obj is Participacao p && ChurrascoAgendado.Equals(p.ChurrascoAgendado) && Usuario.Equals(p.Usuario);
        public override int GetHashCode() =>
            UsuarioEmail.GetHashCode() ^ ChurrascoAgendadoDia ^ ChurrascoAgendadoMes ^ ChurrascoAgendadoAno;
    }
}
