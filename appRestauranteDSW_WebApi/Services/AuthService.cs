using Microsoft.EntityFrameworkCore;
using appRestauranteDSW_WebApi.Data.Entities;
using appRestauranteDSW_WebApi.DTOs;

namespace appRestauranteDSW_WebApi.Services
{
    public class AuthService
    {
        private readonly RestauranteContext _ctx;
        private readonly TokenService _token;
        private readonly EmailService _email;

        public AuthService(RestauranteContext ctx, TokenService token, EmailService email)
        {
            _ctx = ctx;
            _token = token;
            _email = email;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            // 1) Busca usuario por correo
            var user = await _ctx.usuario
                .FirstOrDefaultAsync(u => u.correo == request.Correo);

            if (user == null) return null;

            // 2) Verifica contraseña
            if (!BCrypt.Net.BCrypt.Verify(request.Contrasena, user.contrasena)) return null;

            //Validar si el Usuario esta activo
            if (user.verificado != true)
                return null;

            // 3) Descubre rol desde Empleado->Cargo (si existe)
            var empleado = await _ctx.empleado
                .Include(e => e.cargo)
                .FirstOrDefaultAsync(e => e.usuario_id == user.id);

            var rol = empleado?.cargo?.nombre ?? "Empleado";

            // 4) Genera token
            var (token, exp) = _token.Generate(user.id.ToString(), user.correo!, rol);

            return new LoginResponse
            {
                Token = token,
                Usuario = user.correo!,
                Rol = rol,
                EmpleadoId = empleado?.id ?? 0,
                Expira = exp
            };
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var exists = await _ctx.usuario.AnyAsync(u => u.correo == request.Correo);
            if (exists) return new RegisterResponse { Message = "El correo ya está registrado" };

            var token = Guid.NewGuid().ToString();

            var usuario = new usuario
            {
                correo = request.Correo,
                contrasena = BCrypt.Net.BCrypt.HashPassword(request.Contrasena),
                verificado = false,
                token_verificacion = token,
                fecha_token = DateTime.UtcNow.AddHours(24)
            };

            _ctx.usuario.Add(usuario);
            await _ctx.SaveChangesAsync();

            await _email.SendVerificationEmail(usuario.correo, token);

            return new RegisterResponse
            {
                Message = "Registro exitoso, revisa tu correo para verificar la cuenta",
                Id = usuario.id   //  aquí devolvemos el id para usarlo en el registro de empleados
            };
        }

        public async Task<bool> VerifyEmailAsync(string token)
        {
            var usuario = await _ctx.usuario
                .FirstOrDefaultAsync(u => u.token_verificacion == token);

            if (usuario == null || usuario.fecha_token < DateTime.UtcNow) return false;

            usuario.verificado = true;
            usuario.token_verificacion = null;
            usuario.fecha_token = null;

            await _ctx.SaveChangesAsync();
            return true;
        }

    }
}
