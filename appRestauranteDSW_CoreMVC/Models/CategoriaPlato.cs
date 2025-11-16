using System.ComponentModel.DataAnnotations;

namespace appRestauranteDSW_CoreMVC.Models
{
    public class CategoriaPlato
    {
        [Display(Name = "ID Categoría")]
        public string? id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
        [Display(Name = "Nombre Categoría")]
        public string? nombre { get; set; }

        // solo lectura para mostrar platos de la categoría
        public List<Plato>? plato { get; set; }
    }
}
