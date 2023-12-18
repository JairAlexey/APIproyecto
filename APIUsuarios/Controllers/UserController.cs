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

        // POST api/<UserController> //GUARDAR INFORMACION
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user) //Se guarda igual con un objeto
        {
            User usuarioEncontrado = await _db.Users.FirstOrDefaultAsync(x => x.IdUsuario == user.IdUsuario); //Primero buscamos si ya existe un USUARIO con ese ID
            if (usuarioEncontrado == null && user != null) //Si no hay un usuario con el mismo ID y es diferente de nul, se guarda
            {
                await _db.Users.AddAsync(user);//Proceso para guardado
                await _db.SaveChangesAsync();
                return Ok(user);
            }

            //**usuarioEncontrado** es una variable local por ello solo se encuentra en el metodo que se la declara

            return BadRequest("No se pudo crear el usuario");
        }


        // PUT api/<UsuarioController>/5 //Actualizar
        [HttpPut("{IdUsuario}")]
        public async Task<IActionResult> Put(int IdUsuario, [FromBody] User user) //PUT, POST Y DELETE se lleva los datos en la URL como parametros, FromDody los datos se mandan los datos en el cuerpo del mensaje.
        {
            User usuarioEncontrado = await _db.Users.FirstOrDefaultAsync(x => x.IdUsuario == IdUsuario); //Primero buscamos si ya existe un USUARIO con ese ID
            if (usuarioEncontrado != null)
            {
                usuarioEncontrado.Correo = user.Correo != null ? user.Correo : usuarioEncontrado.Correo; //Se valida que no es nulo, caso contrario se queda con el mismo
                usuarioEncontrado.Clave = user.Clave != null ? user.Clave : usuarioEncontrado.Clave;//lo mismo


                _db.Users.Update(usuarioEncontrado);
                await _db.SaveChangesAsync();
                return Ok(usuarioEncontrado);
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
            try
            {
                string username = request.Correo;
                string password = request.Clave;

                User usuario = await _db.Users.FirstOrDefaultAsync(x => x.Correo == username && x.Clave == password);

                if (usuario != null)
                {
                    // Inicio de sesión exitoso
                    return Ok(new { message = "Inicio de sesión exitoso", usuario.IdUsuario });
                }
                else
                {
                    // Credenciales inválidas, el usuario no existe o la contraseña es incorrecta
                    return Unauthorized(new { message = "Credenciales inválidas. Intente nuevamente." });
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, new { message = "Error interno del servidor." });
            }
        }

        [HttpGet("{IdUsuario}/citas")]
        public IActionResult GetCitasByUsuario(int IdUsuario)
        {
            try
            {
                var citas = _db.Citas.Where(c => c.IdUsuario == IdUsuario).ToList();
                return Ok(citas);
            }
            catch (Exception ex)
            {

                // Devuelve un mensaje de error detallado al cliente
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }




    }
}
