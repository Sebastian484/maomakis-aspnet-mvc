using appRestauranteDSW_WebApi.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] // requiere JWT
    public class CategoriasController : ControllerBase
    {
        private readonly RestauranteContext _ctx;
        public CategoriasController(RestauranteContext ctx) => _ctx = ctx;

        // GET: api/categorias
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categorias = await _ctx.categoria_plato
                .Include(c => c.plato)
                .ToListAsync();
            return Ok(categorias);
        }

        // GET: api/categorias/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var categoria = await _ctx.categoria_plato
                .Include(c => c.plato)
                .FirstOrDefaultAsync(c => c.id == id);

            if (categoria == null)
                return NotFound(new { message = $"No se encontró la categoría con id {id}" });

            return Ok(categoria);
        }

        // POST: api/categorias
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] categoria_plato categoria)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            // Validar que no exista otra categoría con el mismo nombre
            if (await _ctx.categoria_plato.AnyAsync(c => c.nombre.ToLower() == categoria.nombre.ToLower()))
                return Conflict(new { message = "Ya existe una categoría con ese nombre." });

            // Generar ID correlativo tipo CAT01, CAT02...
            var lastCategoria = await _ctx.categoria_plato
                .OrderByDescending(c => c.id)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastCategoria != null && lastCategoria.id.Length >= 5 && int.TryParse(lastCategoria.id.Substring(3), out int lastNumber))
            {
                nextNumber = lastNumber + 1;
            }

            categoria.id = $"CAT{nextNumber:00}";

            _ctx.categoria_plato.Add(categoria);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Categoría creada exitosamente", data = categoria });
        }


        // PUT: api/categorias/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] categoria_plato categoria)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            var existingCategoria = await _ctx.categoria_plato.FindAsync(id);
            if (existingCategoria == null)
                return NotFound(new { message = $"No se encontró la categoría con id {id}" });

            // Validar duplicado de nombre
            if (await _ctx.categoria_plato.AnyAsync(c => c.nombre.ToLower() == categoria.nombre.ToLower() && c.id != id))
                return BadRequest(new { message = "Ya existe una categoría con ese nombre" });

            existingCategoria.nombre = categoria.nombre;

            await _ctx.SaveChangesAsync();
            return Ok(new { message = "Categoría actualizada correctamente", data = existingCategoria });
        }

        // DELETE: api/categorias/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var categoria = await _ctx.categoria_plato.FindAsync(id);
            if (categoria == null)
                return NotFound(new { message = $"No se encontró la categoría con id {id}" });

            _ctx.categoria_plato.Remove(categoria);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Categoría eliminada correctamente" });
        }
    }
}
