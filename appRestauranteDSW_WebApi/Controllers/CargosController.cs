using appRestauranteDSW_WebApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CargosController : ControllerBase
    {
        private readonly RestauranteContext _ctx;
        public CargosController(RestauranteContext ctx) => _ctx = ctx;

        // GET: api/cargos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cargos = await _ctx.cargo.ToListAsync();
            return Ok(cargos);
        }

        // GET: api/cargos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cargo = await _ctx.cargo.FindAsync(id);
            if (cargo == null)
                return NotFound(new { message = $"No se encontró el cargo con id {id}" });
            return Ok(cargo);
        }

        // POST: api/cargos
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] cargo cargo)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            _ctx.cargo.Add(cargo);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Cargo creado exitosamente", data = cargo });
        }

        // PUT: api/cargos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] cargo cargo)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            var existingCargo = await _ctx.cargo.FindAsync(id);
            if (existingCargo == null)
                return NotFound(new { message = $"No se encontró el cargo con id {id}" });

            // Actualizar campos
            existingCargo.nombre = cargo.nombre;

            await _ctx.SaveChangesAsync();
            return Ok(new { message = "Cargo actualizado correctamente", data = existingCargo });
        }

        // DELETE: api/cargos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cargo = await _ctx.cargo.FindAsync(id);
            if (cargo == null)
                return NotFound(new { message = $"No se encontró el cargo con id {id}" });

            _ctx.cargo.Remove(cargo);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Cargo eliminado correctamente" });
        }
    }
}
