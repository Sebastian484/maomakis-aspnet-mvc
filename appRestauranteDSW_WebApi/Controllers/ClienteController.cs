using appRestauranteDSW_WebApi.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly RestauranteContext _ctx;
        public ClientesController(RestauranteContext ctx) => _ctx = ctx;

        // GET: api/clientes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _ctx.cliente.ToListAsync();
            return Ok(clientes);
        }

        // GET: api/clientes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _ctx.cliente.FindAsync(id);
            if (cliente == null)
                return NotFound(new { message = $"No se encontró el cliente con id {id}" });

            return Ok(cliente);
        }

        // POST: api/clientes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] cliente cliente)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            // Validar duplicado por DNI
            if (!string.IsNullOrEmpty(cliente.dni) &&
                await _ctx.cliente.AnyAsync(c => c.dni == cliente.dni))
                return Conflict(new { message = "Ya existe un cliente con ese DNI." });

            _ctx.cliente.Add(cliente);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Cliente creado exitosamente", data = cliente });
        }

        // PUT: api/clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] cliente cliente)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            var existingCliente = await _ctx.cliente.FindAsync(id);
            if (existingCliente == null)
                return NotFound(new { message = $"No se encontró el cliente con id {id}" });

            // Validar duplicado por DNI
            if (!string.IsNullOrEmpty(cliente.dni) &&
                await _ctx.cliente.AnyAsync(c => c.dni == cliente.dni && c.id != id))
                return BadRequest(new { message = "Ya existe un cliente con ese DNI" });

            existingCliente.nombre = cliente.nombre;
            existingCliente.apellido = cliente.apellido;
            existingCliente.dni = cliente.dni;

            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Cliente actualizado correctamente", data = existingCliente });
        }

        // DELETE: api/clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _ctx.cliente.FindAsync(id);
            if (cliente == null)
                return NotFound(new { message = $"No se encontró el cliente con id {id}" });

            _ctx.cliente.Remove(cliente);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Cliente eliminado correctamente" });
        }
    }
}
