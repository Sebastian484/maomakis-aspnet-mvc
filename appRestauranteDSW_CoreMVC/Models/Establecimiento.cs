using System.ComponentModel.DataAnnotations;

namespace appRestauranteDSW_CoreMVC.Models
{
    public class Establecimiento
    {
        [Display(Name = "Id de Establecimiento")] public string Id { get; set; }
        [Display(Name = "Direccion")] public string Direccion { get; set; }
        [Display(Name = "Nombre")] public string Nombre { get; set; }
        [Display(Name = "RUC")] public string Ruc { get; set; }
        [Display(Name = "Telefono")] public string Telefono { get; set; }
    }
}
