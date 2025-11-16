using appRestauranteDSW_WebApi.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] // requiere JWT
    public class MesasController : ControllerBase
    {
        private readonly RestauranteContext _ctx;
        public MesasController(RestauranteContext ctx) => _ctx = ctx;

        // GET: api/mesas
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var mesas = await _ctx.mesa.ToListAsync();
            return Ok(mesas);
        }

        // GET: api/mesas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var mesa = await _ctx.mesa.FindAsync(id);
            if (mesa == null) return NotFound(new { message = $"No se encontró la mesa con id {id}" });
            return Ok(mesa);
        }

        // POST: api/mesas
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] mesa mesa)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            _ctx.mesa.Add(mesa);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Mesa creada exitosamente", data = mesa });
        }

        // PUT: api/mesas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] mesa mesa)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            var existingMesa = await _ctx.mesa.FindAsync(id);
            if (existingMesa == null)
                return NotFound(new { message = $"No se encontró la mesa con id {id}" });

            // Actualiza los campos
            existingMesa.cantidad_asientos = mesa.cantidad_asientos;
            existingMesa.estado = mesa.estado;

            await _ctx.SaveChangesAsync();
            return Ok(new { message = "Mesa actualizada correctamente", data = existingMesa });
        }

        // DELETE: api/mesas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var mesa = await _ctx.mesa.FindAsync(id);
            if (mesa == null)
                return NotFound(new { message = $"No se encontró la mesa con id {id}" });

            _ctx.mesa.Remove(mesa);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Mesa eliminada correctamente" });
        }

        // GET: api/mesas/disponibles
        [HttpGet("disponibles")]
        public async Task<IActionResult> GetDisponibles()
        {
            var mesas = await _ctx.mesa
                .Where(m => m.estado == "Disponible")
                .ToListAsync();

            if (!mesas.Any())
                return NotFound(new { message = "No hay mesas disponibles" });

            return Ok(mesas);
        }

        // GET: api/mesas/ocupadas
        [HttpGet("ocupadas")]
        public async Task<IActionResult> GetOcupadas()
        {
            var mesas = await _ctx.mesa
                .Where(m => m.estado == "Ocupada")
                .ToListAsync();

            if (!mesas.Any())
                return NotFound(new { message = "No hay mesas ocupadas" });

            return Ok(mesas);
        }

        // PUT: api/mesas/{id}/ocupar
        [HttpPut("{id}/ocupar")]
        public async Task<IActionResult> OcuparMesa(int id)
        {
            var mesa = await _ctx.mesa.FindAsync(id);
            if (mesa == null)
                return NotFound(new { message = $"No se encontró la mesa con id {id}" });

            if (mesa.estado == "Ocupada")
                return BadRequest(new { message = "La mesa ya está ocupada" });

            mesa.estado = "Ocupada";
            await _ctx.SaveChangesAsync();

            return Ok(new { message = $"La mesa {id} fue marcada como ocupada", data = mesa });
        }

        // PUT: api/mesas/{id}/liberar
        [HttpPut("{id}/liberar")]
        public async Task<IActionResult> LiberarMesa(int id)
        {
            var mesa = await _ctx.mesa.FindAsync(id);
            if (mesa == null)
                return NotFound(new { message = $"No se encontró la mesa con id {id}" });

            if (mesa.estado == "Disponible")
                return BadRequest(new { message = "La mesa ya está disponible" });

            mesa.estado = "Disponible";
            await _ctx.SaveChangesAsync();

            return Ok(new { message = $"La mesa {id} fue liberada", data = mesa });
        }

    }
}
