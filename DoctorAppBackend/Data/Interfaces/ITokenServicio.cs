using Models.Entidades;

namespace Data.Interfaces
{
    public interface ITokenServicio
    {
        string CrearToken(Usuario usuario);
    }
}
