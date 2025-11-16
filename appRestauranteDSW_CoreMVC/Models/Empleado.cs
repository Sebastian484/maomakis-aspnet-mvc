using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


namespace appRestauranteDSW_CoreMVC.Models
{
    public class Empleado
    {
        [Display(Name = "ID del Empleado")] public int? id { get; set; }

        [Display(Name = "Nombre")] public string? nombre { get; set; }

        [Display(Name = "Apellido")] public string? apellido { get; set; }

        [Display(Name = "DNI")] public string? dni { get; set; }

        [Display(Name = "Teléfono")] public string? telefono { get; set; }

  
        [Display(Name = "Fecha de Registro")] public DateTime fecha_registro { get; set; } = DateTime.Now;

        [Display(Name = "Cargo ID")] public int? cargo_id { get; set; }

        [Display(Name = "Usuario ID")] public int? usuario_id { get; set; }

        public Cargo? cargo { get; set; }

        public Usuario? usuario { get; set; }
    }
}
