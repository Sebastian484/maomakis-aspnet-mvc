using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using appRestauranteDSW_WebApi.Data.Entities;

namespace appRestauranteDSW_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleComprobanteController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public DetalleComprobanteController(RestauranteContext context)
        {
            _context = context;
        }

        // POST: api/DetalleComprobante → agregar un pago
        [HttpPost]
        public async Task<ActionResult<detalle_comprobante>> PostDetalleComprobante(detalle_comprobante detalle)
        {
            if (detalle == null)
                return BadRequest("El detalle no puede ser nulo.");

            // Validamos si el comprobante existe
            var comprobante = await _context.comprobante.FindAsync(detalle.comprobante_id);
            if (comprobante == null)
                return NotFound($"No existe el comprobante con ID {detalle.comprobante_id}");

            // Validamos si el método de pago existe
            var metodoPago = await _context.metodo_pago.FindAsync(detalle.metodo_pago_id);
            if (metodoPago == null)
                return NotFound($"No existe el método de pago con ID {detalle.metodo_pago_id}");

            // Guardamos el detalle
            _context.detalle_comprobante.Add(detalle);
            await _context.SaveChangesAsync();

            // Opcional: recalcular totales del comprobante si quieres sumarlos aquí
            // var totalPagado = await _context.detalle_comprobante
            //     .Where(d => d.comprobante_id == detalle.comprobante_id)
            //     .SumAsync(d => d.monto_pago);
            // comprobante.precio_total_pagado = totalPagado;
            // _context.comprobante.Update(comprobante);
            // await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetalleComprobante", new { id = detalle.id }, detalle);
        }

        // GET opcional: ver un detalle de pago en particular
        [HttpGet("{id}")]
        public async Task<ActionResult<detalle_comprobante>> GetDetalleComprobante(int id)
        {
            var detalle = await _context.detalle_comprobante
                .Include(d => d.comprobante)
                .Include(d => d.metodo_pago)
                .FirstOrDefaultAsync(d => d.id == id);

            if (detalle == null)
                return NotFound();

            return detalle;
        }

        // DELETE: api/DetalleComprobante/{id} → eliminar un pago
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleComprobante(int id)
        {
            var detalle = await _context.detalle_comprobante.FindAsync(id);
            if (detalle == null)
                return NotFound();

            _context.detalle_comprobante.Remove(detalle);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

