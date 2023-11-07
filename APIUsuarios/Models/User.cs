using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int IdUsuario { get; set; }

    public string Correo { get; set; }

    public string Clave { get; set; }

}

