using ChurrasAPI.Interfaces;
using ChurrasAPI.Models;
using ChurrasAPI.Models.Requests;
using ChurrasAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChurrasAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AgendaController : ControllerBase
    {
        private IService<ChurrascoAgendado> _cService;
        public AgendaController(IService<ChurrascoAgendado> cService) => _cService = cService;

        /// <summary>
        /// Coleção de churrascos agendados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ListarChurrascosAgendados")]
        public ActionResult<IEnumerable<ChurrascoAgendado>> ListarChurrascosAgendados() => 
            Ok((_cService as ChurrascoAgendadoService).Listar());

        /// <summary>
        /// Enumera todos os participantes de um churrasco
        /// </summary>
        /// <param name="evento">data do churrasco</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ListarParticipantes")]
        public ActionResult<IEnumerable<Participante>> ListarParticipantes(Evento evento) =>
            Ok(_cService.Buscar(evento.Dia, evento.Mes, evento.Ano)?.Participacoes.Select(p => new Participante
            {
                Nome = p.Usuario.Nome,
                Email = p.UsuarioEmail,
                Contribuicao = p.Contribuicao
            }));

        /// <summary>
        /// Marca presença de um usuário em um churrasco
        /// </summary>
        /// <param name="request">Objeto do tipo 'ConfirmarParticipacaoRequest' com a data do churrasco e o email do usuário</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConfirmarParticipacao")]
        public async Task<IActionResult> ConfirmarParticipacao(ConfirmarParticipacaoRequest request)
        {
            if(request is null)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                await (_cService as ChurrascoAgendadoService).ConfirmarParticipacao(request);
                return Ok("Participação confirmada.");
            }
            catch (Exception e)
            {
                if(e is KeyNotFoundException)
                {
                    return NotFound(e.Message);
                }
                else if(e is InvalidOperationException)
                {
                    return Conflict(e.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Cancela a participação de um usuário em um churrasco
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("CancelarParticipacao")]
        public async Task<IActionResult> CancelarParticipacao(ParticipacaoRequest request)
        {
            if (request is null)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                await (_cService as ChurrascoAgendadoService).CancelarParticipacao(request);
                return Ok("Usuário removido");
            }
            catch (Exception e)
            {
                return (e is KeyNotFoundException || e is InvalidOperationException) ? 
                    NotFound(e.Message) : StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Agenda um churrasco em uma data específica
        /// </summary>
        /// <param name="churrasco">Objeto do tipo 'Churrasco'</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AgendarChurrasco")]
        public ActionResult AgendarChurrasco(Churrasco churrasco)
        {
            try
            {
                var churrascoAgendado = new ChurrascoAgendado(churrasco);
                _cService.Criar(churrascoAgendado);
                return Ok("Churrasco agendado :D");
            }
            catch(Exception e)
            {
                if(e is ArgumentNullException)
                {
                    return BadRequest(e.Message);
                }
                else if(e is ArgumentException)
                {
                    return UnprocessableEntity(e.Message);
                }
                else if(e is InvalidOperationException)
                {
                    return Conflict(e.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
