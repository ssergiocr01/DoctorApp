using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class RegistroDto
    {
        [Required(ErrorMessage = "Username es requerido")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password es requerido")]
        public string Password { get; set; }
    }
}
