using Models.Entidades;

namespace Data.Interfaces
{
    public interface ITokenServicio
    {
        Task<string> CrearToken(UsuarioAplicacion usuario);
    }
}
