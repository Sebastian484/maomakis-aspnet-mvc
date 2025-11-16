using System.ComponentModel.DataAnnotations;

namespace appRestauranteDSW_CoreMVC.Models
{
    public class Usuario
    {
        [Display(Name ="Id del Usuario")]public int? id { get; set; }

        [Display(Name = "Código del Usuario")] public int? codigo { get; set; }

        [Display(Name = "Contraseña del Usuario")] public string? contrasena { get; set; }

        [Display(Name = "Correo del Usuario")] public string? correo { get; set; }

        [Display(Name = "Activo")] public bool? verificado { get; set; }
    }

}
