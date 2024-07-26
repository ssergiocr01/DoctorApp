using Microsoft.AspNetCore.Identity;

namespace Models.Entidades
{
    public class RolAplicacion : IdentityRole<int>
    {
        public ICollection<RolUsuarioAplicacion> RolUsuarios { get; set; }
    }
}
