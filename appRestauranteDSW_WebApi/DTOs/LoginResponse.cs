namespace appRestauranteDSW_WebApi.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; } = "";
        public string Usuario { get; set; } = "";
        public string Rol { get; set; } = "Empleado";
        public int EmpleadoId { get; set; }
        public DateTime Expira { get; set; }
    }
}
