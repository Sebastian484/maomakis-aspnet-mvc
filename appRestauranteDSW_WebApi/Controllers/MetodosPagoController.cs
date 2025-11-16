using appRestauranteDSW_WebApi.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class MetodosPagoController : ControllerBase
    {
        private readonly RestauranteContext _ctx;
        public MetodosPagoController(RestauranteContext ctx) => _ctx = ctx;

        // GET: api/metodospago
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var metodos = await _ctx.metodo_pago.ToListAsync();
            return Ok(metodos);
        }

        // GET: api/metodospago/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var metodo = await _ctx.metodo_pago.FindAsync(id);
            if (metodo == null)
                return NotFound(new { message = $"No se encontró el método con id {id}" });

            return Ok(metodo);
        }

        // POST: api/metodospago
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] metodo_pago metodo)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            // Validar que no exista otro método con el mismo nombre
            if (await _ctx.metodo_pago.AnyAsync(m => m.metodo!.ToLower() == metodo.metodo!.ToLower()))
                return Conflict(new { message = "Ya existe un método de pago con ese nombre." });

            _ctx.metodo_pago.Add(metodo);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Método de pago creado exitosamente", data = metodo });
        }

        // PUT: api/metodospago/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] metodo_pago metodo)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            var existingMetodo = await _ctx.metodo_pago.FindAsync(id);
            if (existingMetodo == null)
                return NotFound(new { message = $"No se encontró el método con id {id}" });

            // Validar duplicados
            if (await _ctx.metodo_pago.AnyAsync(m => m.metodo!.ToLower() == metodo.metodo!.ToLower() && m.id != id))
                return BadRequest(new { message = "Ya existe un método de pago con ese nombre" });

            existingMetodo.metodo = metodo.metodo;

            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Método de pago actualizado correctamente", data = existingMetodo });
        }

        // DELETE: api/metodospago/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var metodo = await _ctx.metodo_pago.FindAsync(id);
            if (metodo == null)
                return NotFound(new { message = $"No se encontró el método con id {id}" });

            _ctx.metodo_pago.Remove(metodo);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Método de pago eliminado correctamente" });
        }
    }
}

