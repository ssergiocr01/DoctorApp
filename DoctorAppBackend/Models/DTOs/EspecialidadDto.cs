using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class EspecialidadDto
    {        
        public int Id { get; set; }

        [Required(ErrorMessage ="El Nombre es requerido")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "El Nombre debe ser mínimo de 1 y tener un máximo de 60 caracteres")]
        public string NombreEspecialidad { get; set; }

        [Required(ErrorMessage = "La Descripción es requerido")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El Nombre debe ser mínimo de 1 y tener un máximo de 100 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage ="El estado es requerido")]
        public int Estado { get; set; }
    }
}
