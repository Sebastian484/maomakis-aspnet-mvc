using appRestauranteDSW_WebApi.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ComprobantesController : ControllerBase
    {
        private readonly RestauranteContext _ctx;
        public ComprobantesController(RestauranteContext ctx) => _ctx = ctx;

        // GET: api/comprobantes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comprobantes = await _ctx.comprobante
                .Select(c => new
                {
                    c.id,
                    c.fecha_emision,
                    c.cliente_id,
                    c.empleado_id,
                    c.tipo_comprobante_id,
                    c.comanda_id,
                    c.igv_total,
                    c.precio_total_pedido,
                    c.sub_total,

                })
                .ToListAsync();

            return Ok(comprobantes);
        }


        // GET: api/comprobantes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var comprobante = await _ctx.comprobante
                .Include(c => c.cliente)
                .Include(c => c.empleado)
                .Include(c => c.tipo_comprobante)
                .Include(c => c.comanda)
                .FirstOrDefaultAsync(c => c.id == id);

            if (comprobante == null)
                return NotFound(new { message = $"No se encontró el comprobante con id {id}" });

            return Ok(comprobante);
        }

        // POST: api/comprobantes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] comprobante comprobante)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            // Opcional: podrías validar que cliente, empleado, comanda existan
            if (comprobante.cliente_id.HasValue &&
                !await _ctx.cliente.AnyAsync(c => c.id == comprobante.cliente_id.Value))
                return BadRequest(new { message = "Cliente no válido" });

            if (comprobante.empleado_id.HasValue &&
                !await _ctx.empleado.AnyAsync(e => e.id == comprobante.empleado_id.Value))
                return BadRequest(new { message = "Empleado no válido" });

            if (comprobante.comanda_id.HasValue &&
                !await _ctx.comanda.AnyAsync(cm => cm.id == comprobante.comanda_id.Value))
                return BadRequest(new { message = "Comanda no válida" });

            _ctx.comprobante.Add(comprobante);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Comprobante creado exitosamente", data = comprobante });
        }

        // PUT: api/comprobantes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] comprobante comprobante)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            var existingComprobante = await _ctx.comprobante.FindAsync(id);
            if (existingComprobante == null)
                return NotFound(new { message = $"No se encontró el comprobante con id {id}" });

            // Actualiza campos
            existingComprobante.descuento_total = comprobante.descuento_total;
            existingComprobante.igv_total = comprobante.igv_total;
            existingComprobante.sub_total = comprobante.sub_total;
            existingComprobante.precio_total_pedido = comprobante.precio_total_pedido;
            existingComprobante.caja_id = comprobante.caja_id;
            existingComprobante.cliente_id = comprobante.cliente_id;
            existingComprobante.comanda_id = comprobante.comanda_id;
            existingComprobante.empleado_id = comprobante.empleado_id;
            existingComprobante.tipo_comprobante_id = comprobante.tipo_comprobante_id;
            existingComprobante.fecha_emision = comprobante.fecha_emision;

            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Comprobante actualizado correctamente", data = existingComprobante });
        }

        // DELETE: api/comprobantes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comprobante = await _ctx.comprobante.FindAsync(id);
            if (comprobante == null)
                return NotFound(new { message = $"No se encontró el comprobante con id {id}" });

            _ctx.comprobante.Remove(comprobante);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Comprobante eliminado correctamente" });
        }
    }
}

