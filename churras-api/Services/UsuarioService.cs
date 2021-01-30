using ChurrasAPI.Data;
using ChurrasAPI.Interfaces;
using ChurrasAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChurrasAPI.Services
{
    public class UsuarioService : IService<Usuario>
    {
        private ChurrasDbContext _context;
        private IConfiguration _config;

        public UsuarioService(ChurrasDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public void Criar(Usuario u)
        {
            if (u is null)
            {
                throw new ArgumentNullException("Dados inválidos.");
            }
            else if (_context.Usuarios.Contains(u))
            {
                throw new ArgumentException("Usuário já registrado.");
            }

            _context.Usuarios.Add(u);
            _context.SaveChanges();
        }

        public void Remover(Usuario u)
        {
            if (u is null)
            {
                throw new ArgumentNullException("Dados inválidos.");
            }
            else if (!_context.Usuarios.Contains(u))
            {
                throw new ArgumentException("Usuário não encontrado.");
            }
           
            _context.Usuarios.Remove(u);
            _context.SaveChanges();
        }


        public Usuario Buscar(params object[] keys) => _context.Usuarios.Find(keys);

        public async Task<Usuario> BuscarAsync(params object[] keys) => await _context.Usuarios.FindAsync(keys);

        public string Autenticar(Usuario u)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Secret"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[] { new Claim(ClaimTypes.Name, u.Nome), new Claim(ClaimTypes.Email, u.Email) };

            var token = new JwtSecurityToken(
                 issuer: _config["Issuer"],
                 audience: _config["Audience"],
                 claims: claims,
                 expires: DateTime.Now.AddMinutes(int.Parse(_config["TokenLifeTime"])),
                 signingCredentials: signingCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
