using appRestauranteDSW_WebApi.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstablecimientoController : ControllerBase
    {
        private readonly RestauranteContext _ctx;
        public EstablecimientoController(RestauranteContext ctx)
        {
            _ctx = ctx;
        }

        // GET: api/Establecimiento
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var establecimientos = await _ctx.establecimiento.ToListAsync();
            return Ok(establecimientos);
        }

        // GET: api/Establecimiento/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var est = await _ctx.establecimiento.FindAsync(id);
            if (est == null)
                return NotFound(new { message = $"No se encontró el establecimiento con id {id}" });
            return Ok(est);
        }

        // PUT: api/Establecimiento/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] establecimiento est)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _ctx.establecimiento.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = $"No se encontró el establecimiento con id {id}" });

            // Actualizar campos
            existing.nombre = est.nombre;
            existing.direccion = est.direccion;
            existing.ruc = est.ruc;
            existing.telefono = est.telefono;

            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Establecimiento actualizado correctamente", data = existing });
        }


    }
}