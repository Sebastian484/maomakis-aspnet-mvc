using Newtonsoft.Json;

namespace appRestauranteDSW_CoreMVC.Models
{
    public class Comprobante
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("descuento_total")]
        public decimal? DescuentoTotal { get; set; }

        [JsonProperty("fecha_emision")]
        public DateTime? FechaEmision { get; set; } 

        [JsonProperty("igv_total")]
        public decimal? IgvTotal { get; set; }

        [JsonProperty("precio_total_pedido")]
        public decimal? PrecioTotalPedido { get; set; }

        [JsonProperty("sub_total")]
        public decimal? SubTotal { get; set; }

        [JsonProperty("caja_id")]
        public string? CajaId { get; set; }

        [JsonProperty("cliente_id")]
        public int? ClienteId { get; set; }

        [JsonProperty("comanda_id")]
        public int? ComandaId { get; set; }

        [JsonProperty("empleado_id")]
        public int? EmpleadoId { get; set; }

        [JsonProperty("tipo_comprobante_id")]
        public int? TipoComprobanteId { get; set; }

        // 🔹 Relaciones (opcionales si la API te devuelve objetos anidados)
        public Cliente? Cliente { get; set; }
        public Empleado? Empleado { get; set; }
        public TipoComprobante? TipoComprobante { get; set; }
        public Comanda? Comanda { get; set; }

        public List<DetalleComprobante>? DetalleComprobante { get; set; } = new();
    }
}
