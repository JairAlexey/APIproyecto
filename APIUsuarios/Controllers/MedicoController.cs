using APIUsuarios.Data;
using APIUsuarios.Migrations;
using APIUsuarios.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIUsuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MedicoController : ControllerBase
    {

        private readonly ApplicationDBContext _db; //Declaracion / Solo lectura / Cuando un atributo es privado se le pone "_" (No es necesario)

        public MedicoController(ApplicationDBContext db) //Inyeccion de dependecia
        {
            _db = db;
        }


        // GET: api/<MedicoController> //MOSTRAR INFROMACION / LISTAR
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Medico> medico = await _db.Medicos.ToListAsync();
            return Ok(medico);
        }

        // GET api/<MedicoController>/5 //MOSTRAR INFORMACION POR ID
        [HttpGet("{IdMedico}")] //Para que todo cuadre
        public async Task<IActionResult> Get(int IdMedico)   //el landa es cuando nos vamos en contra del arreglo
        {
            Medico medico = await _db.Medicos.FirstOrDefaultAsync(x => x.IdMedico == IdMedico); //FirstOrDefaultAsync le manda el primero o manda un dato orDefault vacio, busca un arreglo
            if (medico != null)
            {
                return Ok(medico);
            }
            return BadRequest();//Indica que la solucitud de un cliente no se pudo completar. En este caso no se encontro ningun Usuario con el idUsuario proporcionado

        }

        // POST api/<MedicoController> //GUARDAR INFORMACION
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Medico medico) //Se guarda igual con un objeto
        {
            Medico medicoEncontrado = await _db.Medicos.FirstOrDefaultAsync(x => x.IdMedico == medico.IdMedico); //Primero buscamos si ya existe un USUARIO con ese ID
            if (medicoEncontrado == null && medico != null) //Si no hay un usuario con el mismo ID y es diferente de nul, se guarda
            {
                await _db.Medicos.AddAsync(medico);//Proceso para guardado
                await _db.SaveChangesAsync();
                return Ok(medico);
            }

            //**usuarioEncontrado** es una variable local por ello solo se encuentra en el metodo que se la declara

            return BadRequest("No se pudo crear el medico");
        }


        // PUT api/<MedicoController>/5 //Actualizar
        [HttpPut("{IdMedico}")]
        public async Task<IActionResult> Put(int IdMedico, [FromBody] Medico medico) //PUT, POST Y DELETE se lleva los datos en la URL como parametros, FromDody los datos se mandan los datos en el cuerpo del mensaje.
        {
            Medico medicoEncontrado = await _db.Medicos.FirstOrDefaultAsync(x => x.IdMedico == IdMedico); //Primero buscamos si ya existe un USUARIO con ese ID
            if (medicoEncontrado != null)
            {
                medicoEncontrado.Nombre = medico.Nombre != null ? medico.Nombre : medicoEncontrado.Nombre; //Se valida que no es nulo, caso contrario se queda con el mismo
                medicoEncontrado.Apellido = medico.Apellido != null ? medico.Apellido : medicoEncontrado.Apellido;//lo mismo
                medicoEncontrado.Nacionalidad = medico.Nacionalidad != null ? medico.Nacionalidad : medicoEncontrado.Nacionalidad; //Se valida que no es nulo, caso contrario se queda con el mismo
                medicoEncontrado.Especialidad = medico.Especialidad != null ? medico.Especialidad : medicoEncontrado.Especialidad;//lo mismo

                _db.Medicos.Update(medicoEncontrado);
                await _db.SaveChangesAsync();
                return Ok(medicoEncontrado);
            }
            return BadRequest();
        }

        // DELETE api/<MedicoController>/5 //Eliminar
        [HttpDelete("{IdMedico}")]
        public async Task<IActionResult> Delete(int IdMedico)
        {
            Medico medico = await _db.Medicos.FirstOrDefaultAsync(x => x.IdMedico == IdMedico); //Primero buscamos si ya existe un USUARIO con ese ID
            if (medico != null)
            {
                _db.Medicos.Remove(medico);
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

