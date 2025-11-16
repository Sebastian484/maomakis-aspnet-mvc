using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using appRestauranteDSW_WebApi.Data.Entities;

namespace appRestauranteDSW_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleComandaController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public DetalleComandaController(RestauranteContext context)
        {
            _context = context;
        }

        //POST: api/DetalleComanda  → Agregar plato a la comanda
        [HttpPost]
        public async Task<ActionResult<detalle_comanda>> PostDetalleComanda(detalle_comanda detalle)
        {
            if (detalle == null)
                return BadRequest("El detalle no puede ser nulo.");

            // Validamos si la comanda existe
            var comanda = await _context.comanda.FindAsync(detalle.comanda_id);
            if (comanda == null)
                return NotFound($"No existe la comanda con ID {detalle.comanda_id}");

            // Validamos si el plato existe
            var plato = await _context.plato.FindAsync(detalle.plato_id);
            if (plato == null)
                return NotFound($"No existe el plato con ID {detalle.plato_id}");

            // Calculamos precio_unitario si no viene
            if (detalle.precio_unitario == null)
                detalle.precio_unitario = plato.precio_plato;

            // Guardamos el detalle
            _context.detalle_comanda.Add(detalle);
            await _context.SaveChangesAsync();

            //  Recalcular el total de la comanda (sumando todos los detalles)
            var total = await _context.detalle_comanda
                .Where(d => d.comanda_id == detalle.comanda_id)
                .SumAsync(d => d.cantidad_pedido * d.precio_unitario);

            comanda.precio_total = total;
            _context.comanda.Update(comanda);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetalleComanda", new { id = detalle.id }, detalle);
        }


        // ✅ GET (opcional, para ver un detalle en particular)
        [HttpGet("{id}")]
        public async Task<ActionResult<detalle_comanda>> GetDetalleComanda(int id)
        {
            var detalle = await _context.detalle_comanda
                .Include(d => d.plato)
                .Include(d => d.comanda)
                .FirstOrDefaultAsync(d => d.id == id);

            if (detalle == null)
                return NotFound();

            return detalle;
        }

        // ✅ DELETE: api/DetalleComanda/{id} → eliminar un plato de la comanda
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleComanda(int id)
        {
            var detalle = await _context.detalle_comanda.FindAsync(id);
            if (detalle == null)
                return NotFound();

            _context.detalle_comanda.Remove(detalle);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
