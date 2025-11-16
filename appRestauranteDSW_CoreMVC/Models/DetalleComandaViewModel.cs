using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace appRestauranteDSW_CoreMVC.Models
{
    public class DetalleComandaViewModel
    {
        [JsonProperty("comanda_id")]
        public int ComandaId { get; set; }

        [JsonProperty("plato_id")]
        public string PlatoId { get; set; } = null!;

        [JsonProperty("cantidad_pedido")]
        public int Cantidad { get; set; }

        [JsonProperty("precio_unitario")]
        public decimal? PrecioUnitario { get; set; }

        [JsonProperty("observacion")]
        public string? Observacion { get; set; }

        // Extras para la vista
        [JsonProperty("NombrePlato")]
        public string? NombrePlato { get; set; } 

        [JsonProperty("PlatoImagen")]
        public string? PlatoImagen { get; set; }

        [JsonProperty("Subtotal")]
        public decimal Subtotal { get; set; }

    }


}
