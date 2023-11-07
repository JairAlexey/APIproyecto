using APIUsuarios.Data;
using APIUsuarios.Migrations;
using APIUsuarios.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIUsuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {

        private readonly ApplicationDBContext _db; //Declaracion / Solo lectura / Cuando un atributo es privado se le pone "_" (No es necesario)

        public UserController(ApplicationDBContext db) //Inyeccion de dependecia
        {
            _db = db; 
        }


        // GET: api/<UsuarioController> //MOSTRAR INFROMACION / LISTAR
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<User> usuario = await _db.Users.ToListAsync();
            return Ok(usuario);
        }

        // GET api/<UsuarioController>/5 //MOSTRAR INFORMACION POR ID
        [HttpGet("{IdUsuario}")] //Para que todo cuadre
        public async Task<IActionResult> Get(int IdUsuario)   //el landa es cuando nos vamos en contra del arreglo
        {
            User usuario = await _db.Users.FirstOrDefaultAsync(x => x.IdUsuario == IdUsuario); //FirstOrDefaultAsync le manda el primero o manda un dato orDefault vacio, busca un arreglo
            if (usuario != null) {
                return Ok(usuario);
            }
            return BadRequest();//Indica que la solucitud de un cliente no se pudo completar. En este caso no se encontro ningun Usuario con el idUsuario proporcionado

        }

        // POST api/<MedicoController> //GUARDAR INFORMACION
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cita cita)
        {
            // Verificar si ya existe una cita para el médico en la misma hora
            var existingCita = await _db.Citas.FirstOrDefaultAsync(x => x.IdMedico == cita.IdMedico && x.Hora == cita.Hora);
            if (existingCita != null)
            {
                return BadRequest("El médico ya tiene una cita programada a la misma hora.");
            }

            // Obtener el ID de Medico y Usuario a través de tus funciones o lógica específica
            var medicoId = cita.IdMedico; // Asegúrate de tener el ID de médico en la cita
            var usuarioId = cita.IdUsuario; // Asegúrate de tener el ID de usuario en la cita

            Cita citaEncontrada = await _db.Citas.FirstOrDefaultAsync(x => x.IdCita == cita.IdCita);

            if (citaEncontrada == null && cita != null)
            {
                // Asignar el ID de Medico y Usuario
                cita.IdMedico = medicoId;
                cita.IdUsuario = usuarioId;

                await _db.Citas.AddAsync(cita);
                await _db.SaveChangesAsync();
                return Ok(cita);
            }

            return BadRequest("No se pudo crear la cita");
        }


        // PUT api/<MedicoController>/5 //Actualizar
        [HttpPut("{IdCita}")]
        public async Task<IActionResult> Put(int IdCita, [FromBody] Cita cita)
        {
            // Verificar si ya existe una cita para el médico en la misma hora
            var existingCita = await _db.Citas.FirstOrDefaultAsync(x => x.IdMedico == cita.IdMedico && x.Hora == cita.Hora && x.IdCita != IdCita);
            if (existingCita != null)
            {
                return BadRequest("El médico ya tiene una cita programada a la misma hora.");
            }

            // Obtener el ID de Medico y Usuario a través de tus funciones o lógica específica
            var medicoId = cita.IdMedico; // Asegúrate de tener el ID de médico en la cita
            var usuarioId = cita.IdUsuario; // Asegúrate de tener el ID de usuario en la cita

            Cita citaEncontrada = await _db.Citas.FirstOrDefaultAsync(x => x.IdCita == IdCita);

            if (citaEncontrada != null)
            {
                citaEncontrada.Fecha = cita.Fecha != null ? cita.Fecha : citaEncontrada.Fecha;
                citaEncontrada.Hora = cita.Hora != null ? cita.Hora : citaEncontrada.Hora;

                citaEncontrada.Descripcion = cita.Descripcion != null ? cita.Descripcion : citaEncontrada.Descripcion;

                // Actualizar los IDs de Medico y Usuario
                citaEncontrada.IdMedico = medicoId;
                citaEncontrada.IdUsuario = usuarioId;

                _db.Citas.Update(citaEncontrada);
                await _db.SaveChangesAsync();
                return Ok(citaEncontrada);
            }
            return BadRequest();
        }


        // DELETE api/<UsuarioController>/5 //Eliminar
        [HttpDelete("{IdUsuario}")]
        public async Task<IActionResult> Delete(int IdUsuario)
        {
            User usuario = await _db.Users.FirstOrDefaultAsync(x => x.IdUsuario == IdUsuario); //Primero buscamos si ya existe un USUARIO con ese ID
            if (usuario != null)
            {
                _db.Users.Remove(usuario);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return BadRequest("No se encontró ningún usuario con el Id especificado."); // Se agrega el 'return' para devolver un resultado en caso de que la condición no se cumpla.
            }
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            string username = request.Correo;
            string password = request.Clave;

            User usuario = await _db.Users.FirstOrDefaultAsync(x => x.Correo == username && x.Clave == password);

            if (usuario != null)
            {
                // Inicio de sesión exitoso
                return Redirect("/Home/Index"); // Cambia la ruta a la página de inicio de tu aplicación
            }
            else
            {
                // Credenciales inválidas, el usuario no existe o la contraseña es incorrecta
                return BadRequest("Credenciales inválidas. Intente nuevamente.");
            }
        }


    }
}
