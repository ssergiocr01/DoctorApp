using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entidades;
using System.Net;

namespace Api.Controllers
{

    public class UsuarioController : BaseApiController
    {
        private readonly UserManager<UsuarioAplicacion> _userManager;
        private readonly ITokenServicio _tokenServicio;
        private ApiResponse _response;
        private readonly RoleManager<RolAplicacion> _rolManager;

        public UsuarioController(UserManager<UsuarioAplicacion> userManager, ITokenServicio tokenServicio,
            RoleManager<RolAplicacion> roleManager)
        {
            _userManager = userManager;
            _tokenServicio = tokenServicio;
            _response = new();
            _rolManager = roleManager;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        //{
        //    var usuarios = await _db.Usuarios.ToListAsync();
        //    return Ok(usuarios);
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Usuario>> GetUsuario(int id)
        //{
        //    var usuario = await _db.Usuarios.FindAsync(id);
        //    return Ok(usuario);
        //}

        [Authorize(Policy = "AdminRol")]
        [HttpPost("registro")]
        public async Task<ActionResult<UsuarioDto>> Registro(RegistroDto registroDto)
        {
            if (await UsuarioExiste(registroDto.Username))
            {
                return BadRequest("El usuario ya está registrado");
            }

            var usuario = new UsuarioAplicacion
            {
                UserName = registroDto.Username.ToLower(),
                Email = registroDto.Email,
                Apellidos = registroDto.Apellidos,
                Nombres = registroDto.Nombres,

            };

            var resultado = await _userManager.CreateAsync(usuario, registroDto.Password);

            if (!resultado.Succeeded)
            {
                return BadRequest(resultado.Errors);
            }

            var rolResultado = await _userManager.AddToRoleAsync(usuario, registroDto.Rol);

            if (!rolResultado.Succeeded) return BadRequest("Error al agregar el rol al usuario");

            return new UsuarioDto
            {
                Username = usuario.UserName,
                Token = await _tokenServicio.CrearToken(usuario)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDto>> Login(LoginDto loginDto)
        {
            var usuario = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (usuario == null)
            {
                return Unauthorized("Usuario no válido");
            }

            var resultado = await _userManager.CheckPasswordAsync(usuario, loginDto.Password);

            if (!resultado) return Unauthorized("Password no válido");

            return new UsuarioDto
            {
                Username = usuario.UserName,
                Token = await _tokenServicio.CrearToken(usuario)
            };
        }

        [Authorize(Policy = "AdminRol")]
        [HttpGet("ListadoRoles")]
        public IActionResult GetRoles()
        {
            var roles = _rolManager.Roles.Select(r => new { NombreRol = r.Name }).ToList();
            _response.Resultado = roles;
            _response.IsExitoso = true;
            _response.StatusCode = HttpStatusCode.OK;

            return Ok(_response);
        }

        private async Task<bool> UsuarioExiste(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
