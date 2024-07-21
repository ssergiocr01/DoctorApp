using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class RegistroDto
    {
        [Required(ErrorMessage = "Username es requerido")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password es requerido")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "El password debe tener un mínimo de 4 carácteres y un máximo de 10")]
        public string Password { get; set; }
    }
}
