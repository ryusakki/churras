using ChurrasAPI.Interfaces;
using ChurrasAPI.Models;
using ChurrasAPI.Models.Requests;
using ChurrasAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ChurrasAPI.Controllers
{
    public class AuthController : ControllerBase
    {
        private IService<Usuario> _uService;

        public AuthController(IService<Usuario> uService) => _uService = uService;

        /// <summary>
        /// Autentica um usuário na plataforma
        /// </summary>
        /// <param name="request">Credenciais do usuário</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Autenticar")]
        public ActionResult Autenticar([FromBody] AuthRequest request)
        {
            var u = _uService.Buscar(request.Email);
            if(u is null)
            {
                return NotFound("Usuário não encontrado.");
            }
            if(u.Senha != request.Senha)
            {
                return Unauthorized("Credenciais Inválidas.");
            }

            var token = (_uService as UsuarioService).Autenticar(u);
            return Ok(new { token });
        }

        /// <summary>
        /// Cadastro de usuários
        /// </summary>
        /// <param name="usuario">Dados de cadastro</param>
        /// <returns></returns>
        [HttpPost]
        [Route("RegistrarUsuario")]
        public ActionResult RegistrarUsuario([FromBody] Usuario usuario)
        {
            try
            {
                _uService.Criar(usuario);
                return Ok("Registrado");
            }
            catch (Exception e)
            {
                if(e is ArgumentNullException)
                {
                    return BadRequest(e.Message);
                }
                else if(e is ArgumentException)
                {
                    return Conflict(e.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
