using System.ComponentModel.DataAnnotations;

namespace appRestauranteDSW_CoreMVC.Models
{
    public class Mesas
    {
        [Display(Name = "Id de Mesas")] public int? id { get; set; }

        [Required(ErrorMessage = "La cantidad de asientos es obligatoria")]
        [Display(Name = "Cantidad de Asientos")]
        [Range(1, 50, ErrorMessage = "La cantidad de asientos debe estar entre 1 y 50")]
        public int? cantidad_asientos { get; set; }

        [Display(Name = "Estado de la Mesa")] public string? estado { get; set; }
    }
}
