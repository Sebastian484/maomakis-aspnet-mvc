using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace appRestauranteDSW_CoreMVC.Models
{
    public class Comanda
    {
        public int Id { get; set; }

        [JsonProperty("cantidad_asientos")]
        public int? CantidadAsientos { get; set; }

        [StringLength(255)]
        public string? FechaEmision { get; set; }
        [JsonProperty("precio_total")]
        public decimal? PrecioTotal { get; set; }

        public int? EmpleadoId { get; set; }

        [JsonProperty("estado_comanda_id")]
        public int? EstadoComandaId { get; set; }

        [JsonProperty("mesa_id")]
        public int? MesaId { get; set; }

        // 🔹 Nueva propiedad: lista de detalles
        public List<DetalleComandaViewModel> Detalles { get; set; } = new();
    }
}
