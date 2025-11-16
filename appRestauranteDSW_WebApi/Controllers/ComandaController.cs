using appRestauranteDSW_WebApi.Data;
using appRestauranteDSW_WebApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComandasController : ControllerBase
    {
        private readonly RestauranteContext _ctx;
        public ComandasController(RestauranteContext ctx) => _ctx = ctx;

        // GET: api/Comandas
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comandas = await _ctx.comanda.ToListAsync();

            return Ok(comandas);
        }

        // GET: api/Comandas/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var comanda = await _ctx.comanda.FirstOrDefaultAsync(c => c.id == id);

            if (comanda == null)
                return NotFound(new { message = $"No se encontró la comanda con id {id}" });

            return Ok(comanda);
        }



        // POST: api/Comandas
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] comanda comanda)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            // Estado inicial = Pendiente (id = 1)
            comanda.estado_comanda_id = 1;
            comanda.fecha_emision = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            comanda.precio_total = 0; // inicial

            _ctx.comanda.Add(comanda);
            await _ctx.SaveChangesAsync();

            // Cambiar estado de mesa a Ocupada
            if (comanda.mesa_id.HasValue)
            {
                var mesa = await _ctx.mesa.FindAsync(comanda.mesa_id.Value);
                if (mesa != null)
                {
                    mesa.estado = "Ocupada";
                    await _ctx.SaveChangesAsync();
                }
            }

            return Ok(new { message = "Comanda creada exitosamente", data = comanda });
        }

        // GET: api/Comandas/{id}/detalles
        [HttpGet("{id}/detalles")]
        public async Task<IActionResult> GetDetalles(int id)
        {
            var comanda = await _ctx.comanda
                .Include(c => c.detalle_comanda)
                .ThenInclude(d => d.plato)
                .FirstOrDefaultAsync(c => c.id == id);

            if (comanda == null)
                return NotFound(new { message = $"No se encontró la comanda con id {id}" });

            var detalles = comanda.detalle_comanda.Select(d => new
            {
                d.id,
                d.cantidad_pedido,
                d.precio_unitario,
                d.observacion,
                PlatoId = d.plato_id,
                NombrePlato = d.plato != null ? d.plato.nombre : null,
                PlatoImagen = d.plato != null ? d.plato.imagen : null, // 🔹 nuevo
                Subtotal = (d.cantidad_pedido ?? 0) * (d.precio_unitario ?? 0)
            });

            return Ok(detalles);
        }


        // PUT: api/Comandas/{id}/total
        [HttpPut("{id}/total")]
        public async Task<IActionResult> UpdateTotal(int id)
        {
            var comanda = await _ctx.comanda
                .Include(c => c.detalle_comanda)
                .FirstOrDefaultAsync(c => c.id == id);

            if (comanda == null)
                return NotFound(new { message = $"No se encontró la comanda con id {id}" });

            // Calcular el total
            var total = comanda.detalle_comanda.Sum(d =>
                (d.cantidad_pedido ?? 0) * (d.precio_unitario ?? 0));

            comanda.precio_total = total;

            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Total actualizado correctamente", total });
        }

        // PUT: api/Comandas/{id}/estado
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> UpdateEstado(int id, [FromBody] int nuevoEstadoId)
        {
            var comanda = await _ctx.comanda.FindAsync(id);

            if (comanda == null)
                return NotFound(new { message = $"No se encontró la comanda con id {id}" });

            // Actualizamos el estado
            comanda.estado_comanda_id = nuevoEstadoId;

            await _ctx.SaveChangesAsync();

            return Ok(new
            {
                message = $"Estado de la comanda #{id} actualizado correctamente",
                data = comanda
            });
        }

    }
}
