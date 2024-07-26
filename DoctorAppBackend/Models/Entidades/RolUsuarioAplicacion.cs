using Microsoft.AspNetCore.Identity;

namespace Models.Entidades
{
    public class RolUsuarioAplicacion : IdentityUserRole<int>
    {
        public UsuarioAplicacion UsuarioAplicacion { get; set; }
        public RolAplicacion RolAplicacion { get; set; }
    }
}
