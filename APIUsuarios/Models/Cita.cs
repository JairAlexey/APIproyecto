﻿using System.ComponentModel.DataAnnotations;

namespace APIUsuarios.Models
{
    public class Cita
    {
        [Key]
        public int IdCita { get; set; }

        public string Fecha { get; set; }

        public string Hora { get; set; }

        public string Descripcion { get; set; }

        // ID del Medico
        public int IdMedico { get; set; }

        // ID del Usuario
        public int IdUsuario { get; set; }
    }
}
