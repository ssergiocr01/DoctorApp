using Data.Interfaces.IRepositorio;
using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositorios
{
    public class EspecialidadRepositorio : Repositorio<Especialidad>, IEspecialidadRepositorio
    {
        private readonly ApplicationDbContext _db;

        public EspecialidadRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Especialidad especialidad)
        {
            var especialidadDb = _db.Especialidades.FirstOrDefault(e => e.Id == especialidad.Id);

            if (especialidad != null)
            {
                especialidadDb.NombreEspecialidad = especialidad.NombreEspecialidad;
                especialidad.Descripcion = especialidad.Descripcion;
                especialidad.Estado = especialidad.Estado;
                especialidadDb.FechaActualizacion = DateTime.Now;
                _db.SaveChanges();
            }
        }
    }
}
