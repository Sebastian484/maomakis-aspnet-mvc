using System.ComponentModel.DataAnnotations;

namespace appRestauranteDSW_CoreMVC.Models
{
    public class Cliente
    {
        public int id { get; set; }
        public string? apellido { get; set; }

        public string? dni { get; set; }

        public string? nombre { get; set; }
    }
}
