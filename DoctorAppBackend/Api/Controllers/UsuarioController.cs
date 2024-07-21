using Data;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entidades;
using System.Security.Cryptography;
using System.Text;

namespace Api.Controllers
{

    public class UsuarioController : BaseApiController
    {
        private readonly ApplicationDbContext _db;
        private readonly ITokenServicio _tokenServicio;

        public UsuarioController(ApplicationDbContext db, ITokenServicio tokenServicio)
        {
            _db = db;
            _tokenServicio = tokenServicio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _db.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _db.Usuarios.FindAsync(id);
            return Ok(usuario);
        }

        [HttpPost("registro")]
        public async Task<ActionResult<UsuarioDto>> Registro(RegistroDto registroDto)
        {
            if (await UsuarioExiste(registroDto.Username))
            {
                return BadRequest("El usuario ya está registrado");
            }

            using var hmac = new HMACSHA512();

            var usuario = new Usuario
            {
                Username = registroDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registroDto.Password)),
                PasswordSalt = hmac.Key
            };

            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            return new UsuarioDto
            {
                Username = usuario.Username,
                Token = _tokenServicio.CrearToken(usuario)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDto>> Login(LoginDto loginDto)
        {
            var usuario = await _db.Usuarios.SingleOrDefaultAsync(x => x.Username == loginDto.Username);

            if (usuario == null)
            {
                return Unauthorized("Usuario no válido");
            }

            using var hmac = new HMACSHA512(usuario.PasswordSalt);

            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != usuario.PasswordHash[i])
                {
                    return Unauthorized("Password no válido");
                }
            }

            return new UsuarioDto
            {
                Username = usuario.Username,
                Token = _tokenServicio.CrearToken(usuario)
            };
        }

        private async Task<bool> UsuarioExiste(string username)
        {
            return await _db.Usuarios.AnyAsync(x => x.Username == username.ToLower());
        }
    }
}
