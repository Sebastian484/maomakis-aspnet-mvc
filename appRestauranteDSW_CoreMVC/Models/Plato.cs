using System.ComponentModel.DataAnnotations;

namespace appRestauranteDSW_CoreMVC.Models
{
    public class Plato
    {
        [Display(Name = "ID Plato")]
        public string? id { get; set; }

        [Required(ErrorMessage = "El nombre del plato es obligatorio")]
        [Display(Name = "Nombre del Plato")]
        public string? nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.1, 1000, ErrorMessage = "El precio debe ser mayor a 0")]
        [Display(Name = "Precio")]
        public decimal? precio_plato { get; set; }

        [Display(Name = "Imagen")]
        public string? imagen { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoría")]
        [Display(Name = "Categoría")]
        public string? categoria_plato_id { get; set; }

        // Relación
        public CategoriaPlato? categoria_plato { get; set; }
    }
}
