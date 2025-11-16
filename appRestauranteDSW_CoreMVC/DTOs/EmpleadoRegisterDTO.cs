using System.ComponentModel.DataAnnotations;

namespace appRestauranteDSW_CoreMVC.DTOs
{
    public class EmpleadoRegisterDTO
    {
        // Datos del empleado
        [Required] public string nombre { get; set; }
        [Required] public string apellido { get; set; }
        [Required] public string dni { get; set; }
        public string? telefono { get; set; }
        public int? cargo_id { get; set; }

        // Datos del usuario
        [Required] public string correo { get; set; }
        [Required] public string contrasena { get; set; }
    }
}
