using APIUsuarios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace APIUsuarios.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(
            DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        public DbSet<Medico> Medicos { get; set; }

        public DbSet<Cita> Citas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medico>().HasData(

                new Medico
                {
                    IdMedico = 1, //Autoincremental
                    Nombre = "admin@gmail.com",
                    Apellido = "admin",
                    Nacionalidad = "admin",
                    Especialidad = "admin"
                }

                );

            modelBuilder.Entity<User>().HasData(

                new User
                {
                    IdUsuario = 1, //Autoincremental
                    Correo = "admin@gmail.com",
                    Clave = "admin",
                },
                new User
                {
                    IdUsuario = 2,
                    Correo = "alexeyrueda@gmail.com",
                    Clave = "123",
                }
                );
        }


    }
}
//Simpre que se hace una actualizacion primero se hace la migration y luego se actualiza
//Luego para que se quede la tabla Update-Database
