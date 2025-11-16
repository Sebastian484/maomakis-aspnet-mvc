using appRestauranteDSW_WebApi.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] // requiere JWT
    public class PlatosController : ControllerBase
    {
        private readonly RestauranteContext _ctx;
        public PlatosController(RestauranteContext ctx) => _ctx = ctx;

        // GET: api/platos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var platos = await _ctx.plato
                .Include(p => p.categoria_plato)
                .ToListAsync();
            return Ok(platos);
        }


        // GET: api/platos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var plato = await _ctx.plato
                .Include(p => p.categoria_plato)
                .FirstOrDefaultAsync(p => p.id == id);

            if (plato == null) return NotFound();

            return Ok(plato);
        }

        // POST: api/platos
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] plato plato)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            // Validar que no exista otro plato con el mismo nombre
            var existe = await _ctx.plato
                .AnyAsync(p => p.nombre.ToLower() == plato.nombre.ToLower());

            if (existe)
                return Conflict(new { message = "Ya existe un plato con ese nombre." });

            // Generar Guid si es necesario
            plato.id = Guid.NewGuid().ToString();

            _ctx.plato.Add(plato);
            await _ctx.SaveChangesAsync();

            return Ok(new
            {
                message = "Plato creado exitosamente",
                data = plato
            });
        }


        // PUT: api/platos/{id}
[HttpPut("{id}")]
public async Task<IActionResult> Update(string id, [FromBody] plato plato)
{
    if (!ModelState.IsValid)
        return BadRequest(new { message = "Datos inválidos", errors = ModelState });

    // Busca el plato en la base de datos usando el ID de la URI
    var existingPlato = await _ctx.plato.FindAsync(id);
    if (existingPlato == null)
        return NotFound(new { message = $"No se encontró el plato con id {id}" });

    // Validar que no exista otro plato con el mismo nombre
    if (await _ctx.plato.AnyAsync(p => p.nombre == plato.nombre && p.id != id))
        return BadRequest(new { message = "Ya existe un plato con este nombre" });

    // Actualiza los campos del plato existente
    existingPlato.nombre = plato.nombre;
    existingPlato.precio_plato = plato.precio_plato;
    existingPlato.imagen = plato.imagen;
    existingPlato.categoria_plato_id = plato.categoria_plato_id;

    await _ctx.SaveChangesAsync();

    return Ok(new { message = "Plato actualizado correctamente", data = existingPlato });
}


        // DELETE: api/platos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var plato = await _ctx.plato.FindAsync(id);
            if (plato == null)
                return NotFound(new { message = $"No se encontró el plato con id {id}" });

            _ctx.plato.Remove(plato);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Plato eliminado correctamente" });
        }

        // GET: api/platos/categoria/{categoriaId}
        [HttpGet("categoria/{categoriaId}")]
        public async Task<IActionResult> GetByCategoria(string categoriaId)
        {
            var platos = await _ctx.plato
                .Include(p => p.categoria_plato)
                .Where(p => p.categoria_plato_id == categoriaId)
                .ToListAsync();

            if (!platos.Any())
                return NotFound(new { message = $"No se encontraron platos para la categoría {categoriaId}" });

            return Ok(platos);
        }

        // GET api/platos/search?nombre=xxx
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string nombre)
        {
            var platos = await _ctx.plato
                .Where(p => p.nombre.Contains(nombre))
                .Select(p => new { p.id, p.nombre, p.precio_plato })
                .ToListAsync();
            return Ok(platos);
        }


    }
}
