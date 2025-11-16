using appRestauranteDSW_WebApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoComprobanteController : ControllerBase
    {
        private readonly RestauranteContext _ctx;
        public TipoComprobanteController(RestauranteContext ctx) => _ctx = ctx;

        // GET: api/tipocomprobante
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comprobantes = await _ctx.tipo_comprobante.ToListAsync();
            return Ok(comprobantes);
        }

        // GET: api/tipocomprobante/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tipo = await _ctx.tipo_comprobante.FindAsync(id);
            if (tipo == null)
                return NotFound(new { message = $"No se encontró el tipo de comprobante con id {id}" });
            return Ok(tipo);
        }

        // POST: api/tipocomprobante
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] tipo_comprobante tipoComprobante)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            _ctx.tipo_comprobante.Add(tipoComprobante);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Tipo de comprobante creado exitosamente", data = tipoComprobante });
        }

        // PUT: api/tipocomprobante/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] tipo_comprobante tipoComprobante)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            var existingTipo = await _ctx.tipo_comprobante.FindAsync(id);
            if (existingTipo == null)
                return NotFound(new { message = $"No se encontró el tipo de comprobante con id {id}" });

            // Actualizar campo
            existingTipo.tipo = tipoComprobante.tipo;

            await _ctx.SaveChangesAsync();
            return Ok(new { message = "Tipo de comprobante actualizado correctamente", data = existingTipo });
        }

        // DELETE: api/tipocomprobante/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tipo = await _ctx.tipo_comprobante.FindAsync(id);
            if (tipo == null)
                return NotFound(new { message = $"No se encontró el tipo de comprobante con id {id}" });

            _ctx.tipo_comprobante.Remove(tipo);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Tipo de comprobante eliminado correctamente" });
        }
    }
}
