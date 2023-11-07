using APIUsuarios.Data;
using APIUsuarios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace APIUsuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ApplicationDBContext _db;

        public RoleController(ApplicationDBContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Role> roles = await _db.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpGet("{RoleId}")]
        public async Task<IActionResult> Get(int RoleId)
        {
            Role role = await _db.Roles.FirstOrDefaultAsync(x => x.RoleId == RoleId);
            if (role != null)
            {
                return Ok(role);
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Role role)
        {
            // Limitar los roles a 'administrador' y 'usuario'
            if (role.RoleName != "administrador" && role.RoleName != "usuario")
            {
                return BadRequest("El nombre del rol debe ser 'administrador' o 'usuario'.");
            }

            Role roleFound = await _db.Roles.FirstOrDefaultAsync(x => x.RoleId == role.RoleId);
            if (roleFound == null && role != null)
            {
                await _db.Roles.AddAsync(role);
                await _db.SaveChangesAsync();
                return Ok(role);
            }
            return BadRequest("No se pudo crear el rol");
        }

        [HttpPut("{RoleId}")]
        public async Task<IActionResult> Put(int RoleId, [FromBody] Role role)
        {
            Role roleFound = await _db.Roles.FirstOrDefaultAsync(x => x.RoleId == RoleId);
            if (roleFound != null)
            {
                roleFound.RoleName = role.RoleName != null ? role.RoleName : roleFound.RoleName;
                _db.Roles.Update(roleFound);
                await _db.SaveChangesAsync();
                return Ok(roleFound);
            }
            return BadRequest();
        }

        [HttpDelete("{RoleId}")]
        public async Task<IActionResult> Delete(int RoleId)
        {
            Role role = await _db.Roles.FirstOrDefaultAsync(x => x.RoleId == RoleId);
            if (role != null)
            {
                _db.Roles.Remove(role);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return BadRequest("No se encontró ningún rol con el ID especificado.");
            }
        }
    }
}
