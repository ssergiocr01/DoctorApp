using Microsoft.AspNetCore.Identity;

namespace Models.Entidades
{
    public class UsuarioAplicacion : IdentityUser<int>
    {
        public string Apellidos { get; set; }
        public string Nombres { get; set; }

        public ICollection<RolUsuarioAplicacion> RolUsuarios { get; set; }
    }
}
