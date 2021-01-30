using ChurrasAPI.Data;
using ChurrasAPI.Interfaces;
using ChurrasAPI.Models;
using ChurrasAPI.Models.Requests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurrasAPI.Services
{
    /// <summary>
    /// Serviço responsável por gerenciar os churrascos agendados
    /// </summary>
    public class ChurrascoAgendadoService : IService<ChurrascoAgendado>
    {
        private ChurrasDbContext _context;

        public ChurrascoAgendadoService(ChurrasDbContext context) => _context = context;

        public void Criar(ChurrascoAgendado churrascoAgendado)
        {
            var date = new DateTime(churrascoAgendado.Ano, churrascoAgendado.Mes, churrascoAgendado.Dia);
            
            //Validações
            //Eu não vi nenhuma menção à horários no diagrama do figma, então deduzi que só é possível marcar um churrasco por dia
            if (churrascoAgendado is null)
            {
                throw new ArgumentNullException("Dados inválidos.");
            }
            else if(date.Date < DateTime.Now.Date)  //Tentou agendar o churasco em uma data que já passou...
            {
                throw new ArgumentException("Nosso sistema ainda não suporta viagens no tempo ;[");
            }
            else if (churrascoAgendado.Dia > DateTime.DaysInMonth(churrascoAgendado.Ano, churrascoAgendado.Mes))
            {
                throw new ArgumentException("Dia do agendamento inválido.");
            }
            else if(_context.ChurrascosAgendados.Contains(churrascoAgendado))
            {
                throw new InvalidOperationException("Já existe um churrasco agendado no dia especificado.");
            }

            _context.ChurrascosAgendados.Add(churrascoAgendado);
            _context.SaveChanges();
        } 

        public void Remover(ChurrascoAgendado churrascoAgendado)
        {
            if(!_context.ChurrascosAgendados.Contains(churrascoAgendado))
            {
                throw new ArgumentException("Não há nenhum churrasco agendado no dia especificado.");
            }
            _context.ChurrascosAgendados.Remove(churrascoAgendado);
            _context.SaveChanges();
        }

        public ChurrascoAgendado Buscar(params object[] keys) =>
            _context.ChurrascosAgendados.Find(keys);

        public async Task<ChurrascoAgendado> BuscarAsync(params object[] keys) =>
            await _context.ChurrascosAgendados.FindAsync(keys);

        public IEnumerable<ChurrascoAgendado> Listar() =>
            _context.ChurrascosAgendados.AsEnumerable();

        private async Task<(Usuario, ChurrascoAgendado)> ObtemUsuarioEChurrasco(ParticipacaoRequest request)
        {
            var tChurrascoAgendado = _context.ChurrascosAgendados.FindAsync(request.Evento.Dia, request.Evento.Mes, request.Evento.Ano);
            var tUsuario = _context.Usuarios.FindAsync(request.UsuarioEmail);

            var churrascoAgendado = await tChurrascoAgendado;
            if (churrascoAgendado is null)
            {
                throw new KeyNotFoundException("Não há nenhum churrasco agendado para a data especificada.");
            }

            var usuario = await tUsuario;
            if (usuario is null)
            {
                throw new KeyNotFoundException("Não foi possível participar do churrasco: Usuário não encontrado.");
            }
            return (usuario, churrascoAgendado);
        }

        public async Task ConfirmarParticipacao(ConfirmarParticipacaoRequest request)
        {
            var (usuario, churrascoAgendado) = await ObtemUsuarioEChurrasco(request);
            var participacao = new Participacao(usuario, churrascoAgendado, request.Contribuicao);
            if (churrascoAgendado.Participacoes.Contains(participacao))
            {
                throw new InvalidOperationException("Participação já confirmada.");
            }
            churrascoAgendado.Participacoes.Add(participacao);
            _context.SaveChanges();
        }

        public async Task CancelarParticipacao(ParticipacaoRequest request)
        {
            var (usuario, churrascoAgendado) = await ObtemUsuarioEChurrasco(request);
            var participacao = new Participacao(usuario, churrascoAgendado);

            if (!churrascoAgendado.Participacoes.Contains(participacao))
            {
                throw new InvalidOperationException("Participação não confirmada.");
            }

            churrascoAgendado.Participacoes.Remove(participacao);
            _context.SaveChanges();
        }
    }
}
