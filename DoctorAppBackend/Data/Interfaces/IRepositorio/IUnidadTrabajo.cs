namespace Data.Interfaces.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable
    {
        IEspecialidadRepositorio Especialidad { get; }

        Task Guardar();
    }
}
