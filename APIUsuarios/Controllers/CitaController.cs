using APIUsuarios.Data;
using APIUsuarios.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIUsuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CitaController : ControllerBase
    {

        private readonly ApplicationDBContext _db; //Declaracion / Solo lectura / Cuando un atributo es privado se le pone "_" (No es necesario)

        public CitaController(ApplicationDBContext db) //Inyeccion de dependecia
        {
            _db = db;
        }


        // GET: api/<MedicoController> //MOSTRAR INFROMACION / LISTAR
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Cita> cita = await _db.Citas.ToListAsync();
            return Ok(cita);
        }

        // GET api/<MedicoController>/5 //MOSTRAR INFORMACION POR ID
        [HttpGet("{IdCita}")] //Para que todo cuadre
        public async Task<IActionResult> Get(int IdCita)   //el landa es cuando nos vamos en contra del arreglo
        {
            Cita cita = await _db.Citas.FirstOrDefaultAsync(x => x.IdCita == IdCita); //FirstOrDefaultAsync le manda el primero o manda un dato orDefault vacio, busca un arreglo
            if (cita != null)
            {
                return Ok(cita);
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




        // DELETE api/<MedicoController>/5 //Eliminar
        [HttpDelete("{IdCita}")]
        public async Task<IActionResult> Delete(int IdCita)
        {
            Cita cita = await _db.Citas.FirstOrDefaultAsync(x => x.IdCita == IdCita); //Primero buscamos si ya existe un USUARIO con ese ID
            if (cita != null)
            {
                _db.Citas.Remove(cita);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return BadRequest("No se encontró ningún usuario con el Id especificado."); // Se agrega el 'return' para devolver un resultado en caso de que la condición no se cumpla.
            }
        }


    }
}