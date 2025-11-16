using appRestauranteDSW_WebApi.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] // requiere JWT
    public class UsuariosController : ControllerBase
    {
        private readonly RestauranteContext _ctx;
        public UsuariosController(RestauranteContext ctx) => _ctx = ctx;

        // GET: api/usuarios
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _ctx.usuario
                .Include(u => u.empleado)
                .ToListAsync();

            return Ok(usuarios);
        }

        // GET: api/usuarios/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _ctx.usuario
                .Include(u => u.empleado)
                .FirstOrDefaultAsync(u => u.id == id);

            if (usuario == null)
                return NotFound(new { message = $"No se encontró el usuario con id {id}" });

            return Ok(usuario);
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] usuario usuario)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            // Validar que no exista otro usuario con el mismo correo
            if (!string.IsNullOrEmpty(usuario.correo))
            {
                var existe = await _ctx.usuario
                    .AnyAsync(u => u.correo!.ToLower() == usuario.correo.ToLower());

                if (existe)
                    return Conflict(new { message = "Ya existe un usuario con ese correo" });
            }

            var maxCodigo = await _ctx.usuario.MaxAsync(u => (int?)u.codigo) ?? 0;
            usuario.codigo = maxCodigo + 1;


            _ctx.usuario.Add(usuario);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Usuario creado exitosamente", data = usuario });
        }

        // PUT: api/usuarios/{id}/desactivar
        [HttpPut("{id}/desactivar")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var usuario = await _ctx.usuario.FindAsync(id);
            if (usuario == null)
                return NotFound(new { message = $"No se encontró el usuario con id {id}" });

            usuario.verificado = false;
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Usuario desactivado correctamente", data = usuario });
        }

    }
}

