using System.ComponentModel.DataAnnotations;

namespace APIUsuarios.Models
{
    public class LoginRequest
    {
        public string Clave { get; set; }
        [Required]
        public string Correo { get; set; }

    }
}
