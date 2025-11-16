using appRestauranteDSW_WebApi.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class EmpleadosController : ControllerBase
    {
        private readonly RestauranteContext _ctx;
        public EmpleadosController(RestauranteContext ctx) => _ctx = ctx;

        // GET: api/empleados
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var empleados = await _ctx.empleado
                .Include(e => e.cargo)
                .Include(e => e.usuario)
                .ToListAsync();
            return Ok(empleados);
        }

        // GET: api/empleados/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var empleado = await _ctx.empleado
                .Include(e => e.cargo)
                .Include(e => e.usuario)
                .FirstOrDefaultAsync(e => e.id == id);

            if (empleado == null)
                return NotFound(new { message = $"No se encontró el empleado con id {id}" });

            return Ok(empleado);
        }

        // POST: api/empleados
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] empleado empleado)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            empleado.fecha_registro = DateTime.UtcNow;

            _ctx.empleado.Add(empleado);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Empleado creado exitosamente", data = empleado });
        }

        // PUT: api/empleados/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] empleado empleado)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            var existingEmpleado = await _ctx.empleado.FindAsync(id);
            if (existingEmpleado == null)
                return NotFound(new { message = $"No se encontró el empleado con id {id}" });

            // Actualiza los campos
            existingEmpleado.nombre = empleado.nombre;
            existingEmpleado.apellido = empleado.apellido;
            existingEmpleado.dni = empleado.dni;
            existingEmpleado.telefono = empleado.telefono;
            existingEmpleado.cargo_id = empleado.cargo_id;
            existingEmpleado.usuario_id = empleado.usuario_id;

            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Empleado actualizado correctamente", data = existingEmpleado });
        }

        // DELETE: api/empleados/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var empleado = await _ctx.empleado.FindAsync(id);
            if (empleado == null)
                return NotFound(new { message = $"No se encontró el empleado con id {id}" });

            _ctx.empleado.Remove(empleado);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Empleado eliminado correctamente" });
        }
    }
}
