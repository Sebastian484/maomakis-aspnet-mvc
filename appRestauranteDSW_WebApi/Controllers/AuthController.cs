using appRestauranteDSW_WebApi.DTOs;
using appRestauranteDSW_WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace appRestauranteDSW_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _auth;

        public AuthController(AuthService auth) => _auth = auth;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var res = await _auth.LoginAsync(request);
            if (res == null) return Unauthorized(new { message = "Credenciales inválidas" });
            return Ok(res);
        }

        //Registro y Verificacion de Usuario por Correo

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var res = await _auth.RegisterAsync(request);
            return Ok(res);
        }

        [HttpGet("verify")]
        public async Task<IActionResult> Verify([FromQuery] string token)
        {
            var success = await _auth.VerifyEmailAsync(token);
            if (!success) return BadRequest(new { message = "Token inválido o expirado" });
            return Ok(new { message = "Cuenta verificada con éxito" });
        }

    }
}
